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
        OnLeave = 4
    }
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public string ApplicationuserId { get; set; }
        public Applicationuser Applicationuser { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; } 

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
