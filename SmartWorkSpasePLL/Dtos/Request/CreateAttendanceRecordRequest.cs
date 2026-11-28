using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Request
{
    public class CreateAttendanceRecordRequest
    {
        public int Id { get; set; }
        public string ApplicationuserId { get; set; }
        public Applicationuser Applicationuser { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public bool IsLeaveDay { get; set; }          // اليوم ده اتحسب من الـ 3 أيام إجازة
        public bool? IsDeleted { get; set; }


        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
