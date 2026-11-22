using SmartWorkSpasePLL.Utilise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpasePLL.Services.Interfaces;
using System.Security.Claims;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ($" {DS.FINANCE_ROLE} , {DS.ADMIN_ROLE}"))]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet]
        [Route("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            var financeUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(financeUserId))
            {
                return Unauthorized("Finance user ID not found.");
            }
            var notifications = await _financeService.GetAllNotification(financeUserId);
            return Ok(notifications);
        }

        [HttpGet]
        [Route("GetNotificationById/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var financeUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(financeUserId))
            {
                return Unauthorized("Finance user ID not found.");
            }
            var notification = await _financeService.GetNotificationById(id,financeUserId);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }
        [HttpGet]
        [Route("AddBasicSalaryToUser")]
        public async Task<IActionResult> AddBasicSalaryToUser(string userId, decimal basicSalary)
        {
            var financeUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(financeUserId))
            {
                return Unauthorized("Finance user ID not found.");
            }
            var result = await _financeService.AddBasicSalaryToUser(userId, financeUserId, basicSalary);
            if (!result)
            {
                return BadRequest("Failed to update basic salary.");
            }
            return Ok("Basic salary updated successfully.");
        }
    }
}
