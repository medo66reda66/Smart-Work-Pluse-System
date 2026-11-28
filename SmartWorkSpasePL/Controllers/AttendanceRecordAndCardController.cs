using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpasePLL.Dtos.Respons;
using SmartWorkSpasePLL.Services.Interfaces;
using System.Security.Claims;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize ]
    public class AttendanceRecordAndCardController : ControllerBase
    {
        private readonly IAttendanceRecordAndCardService _attendanceService;

        public AttendanceRecordAndCardController(IAttendanceRecordAndCardService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("GetAllAttendanceRecords")]
        public async Task<IActionResult> GetAllAttendanceRecords()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var records = await _attendanceService.GetAllAttendanceRecord(userId);

            return Ok(new
            {
                Success = true,
                Message = "Attendance records retrieved successfully.",
                Data = records
            });
        }

        [HttpGet("GetAttendanceRecordById/{id}")]
        public async Task<IActionResult> GetAttendanceRecordById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return NotFound();
            }

            var record = await _attendanceService.GetAttendanceRecordById(id, userId);

            if (record == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Attendance record not found."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Attendance record retrieved successfully.",
                Data = record
            });
        }

        [HttpPost("Checkin")]
        public async Task<IActionResult> Checkin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _attendanceService.Checkin(userId);

            if (result.CheckclosedCheckin == false)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "No Checkin Now"
                });
            }

            if (!result.CheckInOncePerDay)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "You have already checked in today."
                });
            }
           
            if (!result.CheckFineBeforeEnter)
            {
                return BadRequest(new
                {
                    Message = "You have collected 30 black cards. A fine has been applied to your account. Please pay the fine before checking in."
                });
            }

            string Message = "";
            foreach (var card in result.Cards)
            {
                if (card.Color == CardColor.Green)
                {
                    Message = "You checked in before your scheduled arrival time.";
                }
                else if (card.Color == CardColor.Blue)
                {
                    Message = "You arrived one hour late.";
                }
                else if (card.Color == CardColor.Black)
                {
                    if (result.AttendanceRecord.Status == AttendanceStatus.UnpaidAbsence)
                    {
                        Message = "You have exceeded your three allowed paid leave days. Your absence has been recorded as unpaid.";
                    }
                    else
                    {
                        Message = "You arrived more than one hour late. You have received a black card.";
                    }
                }
                else if (card.Color == CardColor.Yellow)
                {
                    Message = "You were absent for the entire working day without attending.";
                }

                break;
            }
            var cardsResponse = result.Cards.Select(c => new CardResponse
            {
                Color = c.Color,
                Source = c.Source,
                Status = c.Status,
                IssuedAt = c.IssuedAt
            }).ToList();

            return Ok(new
            {
                Success = true,
                Message,
                Attendance = new AttendanceRecord
                {
                    Id = result.AttendanceRecord!.Id,
                    Date = result.AttendanceRecord.Date,
                    CheckInTime = result.AttendanceRecord.CheckInTime,
                    CheckOutTime = result.AttendanceRecord.CheckOutTime,
                    Status = result.AttendanceRecord.Status
                },
                Cards = cardsResponse,
            });
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _attendanceService.CheckOut(userId);

            if (result.CheckClosedCheckOut == false)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "No Checkout Now"
                });
            }

            if (!result.CheckOutOncePerDay)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "You have already checked Out today."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Check-Out completed successfully.",
                Attendance = new AttendanceRecord
                {
                    Id = result.CreateAttendanceRecordChecKOut!.Id,
                    Date = result.CreateAttendanceRecordChecKOut.Date,
                    CheckInTime = result.CreateAttendanceRecordChecKOut.CheckInTime,
                    CheckOutTime = result.CreateAttendanceRecordChecKOut.CheckOutTime,
                    Status = result.CreateAttendanceRecordChecKOut.Status
                },
            });

        }
    }
}
