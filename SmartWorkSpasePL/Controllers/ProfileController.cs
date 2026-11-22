using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpasePLL.Dtos.Respons;
using SmartWorkSpasePLL.Services.Interfaces;
using System.Security.Claims;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet("ShowProfile")]
        public async Task<IActionResult> ShowProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var result = await _profileService.ShowProfile(userId);

            if (result == null)
                return NotFound("User not found.");

            return Ok(result);
        }

        // Update Profile
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(
            [FromQuery] UpdateProfileResponse updateProfile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var result = await _profileService.UpdateProfile(
                userId,
                updateProfile
            );

            if (!result)
                return BadRequest("Failed to update profile.");

            return Ok("Profile updated successfully.");
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(
            [FromBody] UpdatePasswordResponse updatePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var result = await _profileService.Updatepassword(
                userId,
                updatePassword
            );

            if (!result)
                return BadRequest("Failed to update password. Check your current password.");

            return Ok("Password updated successfully.");
        }

    }
}
