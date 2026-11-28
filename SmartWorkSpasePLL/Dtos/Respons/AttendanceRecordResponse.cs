using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Respons
{
    public class AttendanceRecordResponse
    {
        public int Id { get; set; }
        public string ApplicationuserId { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus StatusRecord { get; set; }
        public bool IsLeaveDay { get; set; }          // اليوم ده اتحسب من الـ 3 أيام إجازة
        public CardColor Color { get; set; }

        public string Source { get; set; } = string.Empty;

        public CardStatus StatusCard { get; set; }


        public ICollection<CardResponseWithAttendanceRecord>? Cards { get; set; } 
    }
}
