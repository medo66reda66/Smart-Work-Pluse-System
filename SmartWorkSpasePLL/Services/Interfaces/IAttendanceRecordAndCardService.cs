using SmartWorkSpaseADL.Entities;
using SmartWorkSpasePLL.Dtos.Respons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface IAttendanceRecordAndCardService
    {
        Task<List<AttendanceRecordResponse>> GetAllAttendanceRecord(string userId);
         Task<AttendanceRecordResponse?> GetAttendanceRecordById(int id, string userid);


        Task<bool> CheckInOncePerDay(string userId);
        Task<bool> CkeckFineBeforInter(string userId);
        Task<(AttendanceStatus Status, bool IsLeaveDay)> GetAbsenceStatus(string userId);
        Task<AttendanceRecord> CreateAttendanceRecordChecKin(string userid);
        Task<(CardColor color, string sourcetype, AttendanceRecord record)> GetColerCard(AttendanceRecord record);
           Task<List<Card>> CreateCards(string userid, AttendanceRecord record);
           Task<bool> ClosedCheckin();
           Task<CheckinResponse> Checkin(string userId);
           Task RunDailyCheckin();

          Task<bool> CheckOutOncePerDay(string userId);
          Task<bool> ClosedCheckOut();
          Task<AttendanceRecord> CreateAttendanceRecordChecKOut(string userid);
          Task<CheckOutResponse> CheckOut(string userid);

    }
}
