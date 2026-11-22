using Microsoft.AspNetCore.Identity;
using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Dtos.Respons.Result;
using SmartWorkSpasePLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class ApplicationuserAuthServise : IApplicationuserAuthServise
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Applicationuser> _userManager;
        private readonly SignInManager<Applicationuser> _signInManager;
        private readonly ITokenService _tokenService;

        public ApplicationuserAuthServise(AppDbContext context, UserManager<Applicationuser> userManager, SignInManager<Applicationuser> signInManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserRequest request)
        {
            
            if (request.Role == Role.Employee || request.Role == Role.manager)
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null )
                {
                    return false;
                }
            }
            string managerId = null;
            if (request.Role == Role.Hr || request.Role == Role.Finance || request.Role == Role.manager)
            {
                var AdminUser = await _userManager.FindByEmailAsync("Admin123@Hoda.com");
                if (AdminUser == null)
                {
                    return false;
                }
                managerId = AdminUser.Id;
                if (request.Role == Role.Hr || request.Role == Role.Finance)
                {
                    var notification = new Notification
                    {
                        ApplicationUserId = AdminUser.Id, // المستلم: Admin
                        Type = "NewEmployee",
                        Message = $"Please add the basic salary for employee {request.FullName}.",
                        IsRead = false,
                        CreatedAt = DateTime.Now,
                        // ربط الإشعار بالموظف المطلوب تحديد راتبه
                        RelatedUserId = null // يمكنك وضع معرف الموظف الجديد هنا إذا كان متاحًا
                    };
                    _context.Notifications.Add(notification);
                }

            }
          
            var newUser = new Applicationuser
            {
                FullName = request.FullName,
                Phone = request.Phone,
                Email = request.Email,
                UserName = request.Email,
                Department = request.Department,
                Gavernate = request.Gavernate,
                City = request.City,
                ManagerId = managerId!,
                gender = request.gender,
                IsActive = true,
            };
            if(request.ImageUrl != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(request.ImageUrl.FileName);
                var filePath = Path.Combine("wwwroot", "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageUrl.CopyToAsync(stream);
                }
                newUser.ImageUrl = fileName;
            }

            if(request.Role == Role.Employee || request.Role == Role.manager)
            {
                var FinanceUser = await _userManager.FindByEmailAsync("finance123@Hoda.com");
                var notification = new Notification
                {
                    ApplicationUserId = FinanceUser!.Id, // المستلم: Finance
                    Type = "NewEmployee",
                    Message = $"Please add the basic salary for employee {newUser.FullName}.",
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                    // ربط الإشعار بالموظف المطلوب تحديد راتبه
                    RelatedUserId = newUser.Id
                };
                _context.Notifications.Add(notification);
            }
            
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded) 
            {
                return false; 
            }
            await _context.SaveChangesAsync();

            if (request.Role == Role.manager)
            {
                await _userManager.AddToRoleAsync(newUser, "manager");
            }
            else if(request.Role == Role.Employee)
            {
                await _userManager.AddToRoleAsync(newUser, "Employee");
            }
            else if(request.Role == Role.Hr)
            {
                await _userManager.AddToRoleAsync(newUser, "Hr");
            }
            else if(request.Role == Role.Finance)
            {
                await _userManager.AddToRoleAsync(newUser, "Finance");
            }
            return result.Succeeded; 
        }

        public async Task<LogingResult> LoginUserAsync(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return new LogingResult { Status = LoginStatus.UserNotFound };
            }
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if(user == null || user.IsActive == false)
            {
                return new LogingResult { Status = LoginStatus.UserNotFound };
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, true);

            if (result.IsLockedOut)
            {
                return new LogingResult { Status = LoginStatus.LockedOut };
            }
            else if (!result.Succeeded)
            {
                return new LogingResult { Status = LoginStatus.InvalidPassword };
            }


            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = _tokenService.CreateToken(claims);
            //var refreshToken = _tokenService.GenerateRefreshToken();

            //user.RefreshToken = refreshToken;
            //user.RefreshTokenExpiration = DateTime.Now.AddDays(7);
            //await _userManager.UpdateAsync(user);

            return new LogingResult
            {
                Token = token,
                Validtoken = "10 minutes",
                Status = LoginStatus.Success,
                //RefreshToken = refreshToken,
                //RefreshTokenTime = "7 day",
            };
        }

    }
}
