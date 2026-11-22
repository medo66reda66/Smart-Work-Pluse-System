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
    [Authorize(Roles = ($" {DS.HR_ROLE} , {DS.ADMIN_ROLE}"))]
    public class HRController : ControllerBase
    {
        private readonly IHrService _hrService;

        public HRController(IHrService hrService)
        {
            _hrService = hrService;
        }
        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            var Hrid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(Hrid))
            {
                return Unauthorized("HR user ID not found.");
            }
            var notifications = await _hrService.GetAllNotification(Hrid);
            return Ok(notifications);
        }
        [HttpGet("GetNotificationById/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var Hrid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(Hrid))
            {
                return Unauthorized("HR user ID not found.");
            }
            var notification = await _hrService.GetNotificationById(id, Hrid);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

    }
}
