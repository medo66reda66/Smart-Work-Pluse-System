using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Repositories.Interfaces;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Dtos.Respons;
using SmartWorkSpasePLL.Services.Interfaces;
using SmartWorkSpasePLL.Utilise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class AttendanceRecordAndCardService : IAttendanceRecordAndCardService
    {
       private readonly IRepository<AttendanceRecord> _attendanceRecordRepository;
        private readonly IAttendanceRecordRepository _attendanceRecord;
       private readonly IRepository<Card> _card;
       private readonly IRepository<Notification> _notification;
        private readonly ICompanySettingService _company;
        private readonly UserManager<Applicationuser> _userManager;

        public AttendanceRecordAndCardService(IRepository<AttendanceRecord> attendanceRecordRepository, IRepository<Card> card, ICompanySettingService company, UserManager<Applicationuser> userManager, IRepository<Notification> notification, IAttendanceRecordRepository attendanceRecord)
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _card = card;
            _company = company;
            _userManager = userManager;
            _notification = notification;
            _attendanceRecord = attendanceRecord;
        }

        public async Task<List<AttendanceRecordResponse>> GetAllAttendanceRecord(string userId)
        {
            var records = await _attendanceRecordRepository.GetAllAsync(
                e => e.ApplicationuserId == userId && e.IsDeleted != true,
                includes: [e=>e.Cards,e => e.Applicationuser]);


            return records
                .OrderByDescending(x => x.Date)
                .Select(r => new AttendanceRecordResponse
            {
                Id = r.Id,
                ApplicationuserId = r.ApplicationuserId,
                FullName = r.Applicationuser.FullName,
                CheckInTime = r.CheckInTime,
                CheckOutTime = r.CheckOutTime,
                Cards = r.Cards.Select(c => new CardResponseWithAttendanceRecord
                {
                    Color = c.Color,
                    IssuedAt = c.IssuedAt,
                }).ToList(),
                Date = r.Date,
                IsLeaveDay = r.IsLeaveDay,
            }).ToList();
        }
        public async Task<AttendanceRecordResponse?> GetAttendanceRecordById(int id,string userid)
        {
            var record = await _attendanceRecordRepository.GetByIdAsync(
               e => e.Id == id &&e.ApplicationuserId == userid && e.IsDeleted != true,
                includes: [e => e.Cards, e => e.Applicationuser]);

            if(record == null) return null;

            return new AttendanceRecordResponse
            {
                Id = record.Id,
                ApplicationuserId = record.ApplicationuserId,
                FullName = record.Applicationuser.FullName,
                CheckInTime = record.CheckInTime,
                CheckOutTime = record.CheckOutTime,
                Date = record.Date,
                IsLeaveDay = record.IsLeaveDay,
                Cards = record.Cards.Select(c => new CardResponseWithAttendanceRecord
                {
                    Color = c.Color,
                    IssuedAt = c.IssuedAt,
                }).ToList(),
            };
        }

        public async Task<bool> CheckInOncePerDay(String userId)
        {
            var allrecords = await _attendanceRecordRepository.GetAllAsync(e => e.ApplicationuserId == userId && e.IsDeleted != true);

            var NoIntry = allrecords.FirstOrDefault(e => e.CheckInTime.HasValue &&  e.CheckInTime!.Value.Date == DateTime.Today);
            if (NoIntry != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CkeckFineBeforInter(string userId)
        {
            var card = await _card.GetAllAsync(e => e.ApplicationUserId == userId && e.Color == CardColor.Black);
            var company =await _company.GetCompanySettingByIdAsync(1);

            if (card.Count >= company.BlackForFine)
            {
                return false;
            }
            return true;
        }

        public async Task<(AttendanceStatus Status, bool IsLeaveDay)> GetAbsenceStatus(string userId)
        {
            var company = await _company.GetCompanySettingByIdAsync(1);

            var time = DateTime.Now.TimeOfDay;
            AttendanceStatus status = AttendanceStatus.Absent;
            bool isLeaveDay = false;
            if (time <= company.WorkStartTime)
            {
                status = AttendanceStatus.Present;
            }
            else if (time <= company.WorkStartTime.Add(TimeSpan.FromHours(1)))
            {
                status = AttendanceStatus.Late;
            }
            else if (time > company.WorkStartTime.Add(TimeSpan.FromHours(1)) && time <= company.WorkStartTime.Add(TimeSpan.FromHours(4)))
            {
                status = AttendanceStatus.Verylate;
            }
            else if (time > company.WorkEndTime.Subtract(TimeSpan.FromHours(1)) && time < company.WorkEndTime)
            {
                var last30Records = await _attendanceRecord.GetLast30RecordsAsync(userId);
                           
                var countIsLeaveDay = last30Records.Count(e => e.IsLeaveDay == true);

                if (countIsLeaveDay >= 3)
                {
                    status = AttendanceStatus.UnpaidAbsence;
                }
                else
                {
                    status = AttendanceStatus.Absent;
                    isLeaveDay = true;
                }
            }

            return (status, isLeaveDay);
        }

        public async Task<AttendanceRecord> CreateAttendanceRecordChecKin(string userid)
        {
            var getAbsenceStatus = await GetAbsenceStatus(userid);

            var record = new AttendanceRecord
            {
                ApplicationuserId = userid,
                CheckInTime = DateTime.Now,
                Date = DateTime.Today,
                Status = getAbsenceStatus.Status,
                IsLeaveDay = getAbsenceStatus.IsLeaveDay,
            };

            _attendanceRecordRepository.Add(record);
           await _attendanceRecordRepository.SaveChangesAsync();

            return record;
        }

        public async Task<(CardColor color, string sourcetype , AttendanceRecord record)> GetColerCard(AttendanceRecord record)
        {

            CardColor color;
            string sourcetype;
            if (record.Status == AttendanceStatus.Present)
            {
                color = CardColor.Green;
                sourcetype = "EarlyOrOnTime";
            }
            else if (record.Status == AttendanceStatus.Late)
            {
                color = CardColor.Blue;
                sourcetype = "LateWithinOneHour";
            }
            else if (record.Status == AttendanceStatus.Verylate)
            {
                color = CardColor.Black;
                sourcetype = "LateMoreThanOneHour";
            }
            else if (record.Status == AttendanceStatus.UnpaidAbsence)
            {
                color = CardColor.Black;
                sourcetype = "UnpaidAbsence";
            }
            else
            {
                color = CardColor.Yellow;
                sourcetype = "paidAbsenc";
            }

            return (color, sourcetype, record);
        }

        public async Task<List<Card>> CreateCards(string userid , AttendanceRecord record)
        {
           var getColerCard = await GetColerCard(record);
            var Company = await _company.GetCompanySettingByIdAsync(1);

            var cards = new List<Card>();

            if (getColerCard.record.Status == AttendanceStatus.UnpaidAbsence)
            {
                for (int i = 0; i < Company.BlackCardsPerAbsence; i++)
                {
                    var card = new Card
                    {
                        ApplicationUserId = userid,
                        AttendanceRecordId = getColerCard.record.Id,
                        IssuedAt = DateTime.Now,
                        Status = CardStatus.Active,
                        Color = getColerCard.color,
                        Source = getColerCard.sourcetype

                    };
                    cards.Add(card);
                }
            }
            else
            {
                var card = new Card
                {
                    ApplicationUserId = userid,
                    AttendanceRecordId = getColerCard.record.Id,
                    IssuedAt = DateTime.Now,
                    Status = CardStatus.Active,
                    Color = getColerCard.color,
                    Source = getColerCard.sourcetype

                };
                cards.Add(card);
            }

            foreach (var card in cards)
            {
                _card.Add(card);
            }

            await _card.SaveChangesAsync();

            return cards;
        }

        public async Task<bool> ClosedCheckin()
        {
            var Company = await _company.GetCompanySettingByIdAsync(1);
            var Time = DateTime.Now.TimeOfDay;

            if(Time > Company.WorkStartTime.Add(TimeSpan.FromHours(4)) && Time < Company.WorkEndTime.Subtract(TimeSpan.FromHours(1)))
            {
                return false;
            }
            return true;
        }



        public async Task<CheckinResponse> Checkin(string userId)
        {
            var checkclosedCheckin = await ClosedCheckin();
            if(!checkclosedCheckin)
            {
                return new CheckinResponse { CheckclosedCheckin = false};
            }

            var user =await _userManager.FindByIdAsync(userId);

            var checkInOncePerDa = await CheckInOncePerDay(userId);
            if(!checkInOncePerDa)
            {
                return new CheckinResponse { CheckInOncePerDay = false };
            }
           
            var ckeckFineBeforInter =await CkeckFineBeforInter(userId);
            if(!ckeckFineBeforInter)
            {
                return new CheckinResponse { CheckFineBeforeEnter = false };
            }

            var createAttendanceRecord = await CreateAttendanceRecordChecKin(userId);
            
            var createCards = await CreateCards(userId, createAttendanceRecord);

            var hrUsers = await _userManager.GetUsersInRoleAsync(DS.HR_ROLE);
            foreach (var hruser in hrUsers)
            {
                var notification = new Notification
                {
                    ApplicationUserId = hruser.Id,
                    Type = "Attendance",
                    Message = $"Employee: {user!.FullName} | Check-in Time: {createAttendanceRecord.CheckInTime} " +
                    $"| Attendance Status: {createAttendanceRecord.Status} | Total Cards: {createCards.Count}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    RelatedUserId = userId
                };
                _notification.Add(notification);
            }
          await _notification.SaveChangesAsync();

            return new CheckinResponse
            {
                CheckclosedCheckin = checkclosedCheckin,
                CheckInOncePerDay = checkInOncePerDa,
                CheckFineBeforeEnter = ckeckFineBeforInter,
                AttendanceRecord = createAttendanceRecord,
                Cards = createCards
            };
        }
        public async Task RunDailyCheckin()
        {
            var employees = await _userManager.Users
               .Where(u => u.IsActive)
               .ToListAsync();

            foreach (var employee in employees)
            {
                if (!employee.IsActive)
                    continue;

                var canCheckIn = await CheckInOncePerDay(employee.Id);

                if (!canCheckIn)
                    continue;

                await Checkin(employee.Id);
            }
        }
        public async Task<bool> CheckOutOncePerDay(string userId)
        {
            var allrecords = await _attendanceRecordRepository.GetAllAsync(e => e.ApplicationuserId == userId && e.IsDeleted != true);

            var NoIntry = allrecords.FirstOrDefault(e => e.CheckOutTime.HasValue && e.CheckOutTime!.Value.Date == DateTime.Today);
            if (NoIntry != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ClosedCheckOut()
        {
            var Company = await _company.GetCompanySettingByIdAsync(1);
            var Time = DateTime.Now.TimeOfDay;

            if (Time <= Company.WorkEndTime && Time >= Company.WorkEndTime.Subtract(TimeSpan.FromMinutes(10)))
            {
                return true;
            }
            return false;
        }
        public async Task<AttendanceRecord> CreateAttendanceRecordChecKOut(string userid)
        {

            var record = new AttendanceRecord
            {
                ApplicationuserId = userid,
                CheckOutTime = DateTime.Now,
                Date = DateTime.Today,
                Status = AttendanceStatus.OnLeave,
                IsLeaveDay = false,
            };

            _attendanceRecordRepository.Add(record);
            await _attendanceRecordRepository.SaveChangesAsync();

            return record;
        }
        public async Task<CheckOutResponse> CheckOut(string userid)
        {
            var checkClosedCheckOut = await ClosedCheckOut();
            if (!checkClosedCheckOut)
            {
                return new CheckOutResponse { CheckClosedCheckOut = false };
            }

            var user = await _userManager.FindByIdAsync(userid);

            var checkOutOncePerDay = await CheckOutOncePerDay(userid);
            if (!checkOutOncePerDay)
            {
                return new CheckOutResponse { CheckOutOncePerDay = false };
            }

            var createAttendanceRecordChecKOut =await CreateAttendanceRecordChecKOut(userid);

            var hrUsers = await _userManager.GetUsersInRoleAsync(DS.HR_ROLE);
            foreach (var hruser in hrUsers)
            {
                var notification = new Notification
                {
                    ApplicationUserId = userid,
                    Type = "Leave",
                    Message = $"Employee: {user!.FullName} | Check-OUT Time: {createAttendanceRecordChecKOut.CheckOutTime} " +
                    $"| Leave Status: {createAttendanceRecordChecKOut.Status}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    RelatedUserId = hruser.Id
                };
                _notification.Add(notification);
            }
            await _notification.SaveChangesAsync();

            return new CheckOutResponse
            {
                CheckOutOncePerDay = checkClosedCheckOut,
                CheckClosedCheckOut = checkOutOncePerDay,
                CreateAttendanceRecordChecKOut = createAttendanceRecordChecKOut
            };
        }
    }
}
