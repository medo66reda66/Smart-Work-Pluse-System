using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpasePLL.Services.Interfaces;
using System.Security.Claims;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            var AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(AdminId))
            {
                return Unauthorized("Admin user ID not found.");
            }
            var notifications = await _adminService.GetAllNotification(AdminId);
            return Ok(notifications);
        }
        [HttpGet("GetNotificationById/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(AdminId))
            {
                return Unauthorized("Admin user ID not found.");
            }
            var notification = await _adminService.GetNotificationById(id, AdminId);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpGet("AddBasicSalaryToHRANDFinance")]
        public async Task<IActionResult> AddBasicSalaryToHRANDFinance(string userId, decimal basicSalary)
        {
            var result = await _adminService.AddBasicSalaryToHRANDFinance(userId, basicSalary);
            if (!result)
            {
                return BadRequest("Failed to add basic salary.");
            }
            return Ok("Basic salary added successfully.");
        }
    }
}