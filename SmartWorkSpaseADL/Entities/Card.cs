using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum CardStatus
    {
        Active = 1,
        Cancelled = 2,
        Settled = 3
    }
    public class Card
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public Applicationuser ApplicationUser { get; set; } = null!;
        public int AttendanceRecordId { get; set; }
        public AttendanceRecord AttendanceRecord { get; set; } = null!;
        public string Color { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public CardStatus Status { get; set; } 
        public DateTime IssuedAt { get; set; }
    }
}
