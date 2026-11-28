using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 3,
        Verylate = 4,
        OnLeave = 5,
        UnpaidAbsence = 6   // غياب بعد استنفاد الـ 3 أيام
    }
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public string ApplicationuserId { get; set; }
        public Applicationuser Applicationuser { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }= AttendanceStatus.Absent;
        public bool IsLeaveDay { get; set; }          // اليوم ده اتحسب من الـ 3 أيام إجازة
        public bool? IsDeleted { get; set; }


        public ICollection<Card> Cards { get; set; }= new List<Card>();
    }
}
