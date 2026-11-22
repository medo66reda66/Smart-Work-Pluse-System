
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Services.Interfaces;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimeSessionController : ControllerBase
    {
        private readonly IOvertimeSessionService _overtimeSessionService;

        public OvertimeSessionController(
            IOvertimeSessionService overtimeSessionService)
        {
            _overtimeSessionService = overtimeSessionService;
        }

        // POST: api/OvertimeSession
        [HttpPost]
        public async Task<IActionResult> CreateOvertimeSession(
            [FromQuery] string employeeId,
            [FromBody] CreateOvertimeSessionRequest request)
        {
            try
            {
                var result = await _overtimeSessionService
                    .CreateOvertimeSessionAsync(employeeId, request);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/OvertimeSession/my-sessions
        [HttpGet("my-sessions")]
        public async Task<IActionResult> GetMyOvertimeSessions(
            [FromQuery] string employeeId)
        {
            try
            {
                var result = await _overtimeSessionService
                    .GetMyOvertimeSessionsAsync(employeeId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/OvertimeSession/my-sessions/summary
        [HttpGet("my-sessions/summary")]
        public async Task<IActionResult> GetMyOvertimeSessionsSummary(
            [FromQuery] string employeeId)
        {
            try
            {
                var result = await _overtimeSessionService
                    .GetMyOvertimeSessionsSummaryAsync(employeeId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/OvertimeSession/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOvertimeSessionById(
            int id,
            [FromQuery] string employeeId)
        {
            try
            {
                var result = await _overtimeSessionService
                    .GetOvertimeSessionByIdAsync(id, employeeId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/OvertimeSession/{id}/approve
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> ApproveOvertimeSession(
            int id,
            [FromQuery] string approverId)
        {
            try
            {
                var result = await _overtimeSessionService
                    .ApproveOvertimeSessionAsync(id, approverId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/OvertimeSession/{id}/reject
        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> RejectOvertimeSession(
            int id,
            [FromQuery] string approverId)
        {
            try
            {
                var result = await _overtimeSessionService
                    .RejectOvertimeSessionAsync(id, approverId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
