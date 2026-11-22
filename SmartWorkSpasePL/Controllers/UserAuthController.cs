using SmartWorkSpasePLL.Utilise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Services.Interfaces;
using System.Threading.Tasks;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserAuthController : ControllerBase
    {
        private readonly IApplicationuserAuthServise _applicationuserAuth;
        private readonly UserManager<Applicationuser> _userManager;

        public UserAuthController(IApplicationuserAuthServise applicationuserAuth, UserManager<Applicationuser> userManager)
        {
            _applicationuserAuth = applicationuserAuth;
            _userManager = userManager;
        }

        [HttpPost("register")]
        [Authorize(Roles = ($"{DS.HR_ROLE} , {DS.ADMIN_ROLE}"))]
        public  async Task<IActionResult> UserRegister([FromQuery] RegisterUserRequest userRequest)
        {
            var result =await _applicationuserAuth.RegisterUserAsync(userRequest);
            
            return Ok(result);

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> UserLogin([FromQuery] LoginRequest loginUserRequest)
        {
            var result = await _applicationuserAuth.LoginUserAsync(loginUserRequest);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

    }
}
