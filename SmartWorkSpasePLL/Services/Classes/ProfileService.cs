using Microsoft.AspNetCore.Identity;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpasePLL.Dtos.Respons;
using SmartWorkSpasePLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<Applicationuser> _userManager;

        public ProfileService(UserManager<Applicationuser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ShowProfileResponse> ShowProfile(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return null;
            }

            var profile = new ShowProfileResponse
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                BasicSalary = user.BasicSalary,
                Gavernate = user.Gavernate,
                City = user.City,
                gender = user.gender,
                Department = user.Department,
                ImageUrl = user.ImageUrl,
                ManagerId = user.ManagerId,
            };
            return profile;
        }

        public async Task<bool> UpdateProfile(string userid ,UpdateProfileResponse updateProfile)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return false;
            }

            user.FullName = updateProfile.FullName;
            user.Phone = updateProfile.Phone;
            user.gender = updateProfile.gender;
            user.City = updateProfile.City;
            user.Gavernate = updateProfile.Gavernate;

            if(updateProfile.ImageUrl != null)
            {
                var pathimage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var existingImg = Path.Combine(pathimage, user.ImageUrl);
                if (existingImg != null)
                {
                    if (System.IO.File.Exists(existingImg))
                        System.IO.File.Delete(existingImg);
                }

                var mainImageName = Guid.NewGuid() + Path.GetExtension(updateProfile.ImageUrl.FileName);
                var mainImagePath = Path.Combine("wwwroot", "images", mainImageName);
                using (var stream = new FileStream(mainImagePath, FileMode.Create))
                {
                    await updateProfile.ImageUrl.CopyToAsync(stream);
                }
                user.ImageUrl = mainImageName;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Updatepassword(string userid, UpdatePasswordResponse updatePassword)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return false;
            }
            if (updatePassword.ConfirmPassword != null && updatePassword.NewPassword != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, updatePassword.CurrentPassword!);
                if (!passwordCheck)
                {
                    return false;
                }
                var changePassResult = await _userManager.ChangePasswordAsync(user,updatePassword.CurrentPassword!,updatePassword.NewPassword);
                if (!changePassResult.Succeeded)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
