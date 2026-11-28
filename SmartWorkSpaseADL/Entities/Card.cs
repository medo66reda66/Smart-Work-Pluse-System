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
    public enum CardColor
    {
        Green = 1,   // حضر قبل 8
        Blue = 2,    // حضر من 8 لـ 9
        Black = 3 ,   // حضر بعد 9، أو غياب
        Yellow = 4 , // اليوم من 3 ايام في الشهر الي مدفوعين الاجر
    }
    public class Card
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public Applicationuser ApplicationUser { get; set; } = null!;
        public int AttendanceRecordId { get; set; }
        public AttendanceRecord AttendanceRecord { get; set; } = null!;
        public CardColor Color { get; set; }

        public string Source { get; set; } = string.Empty;

        public CardStatus Status { get; set; } 
        public DateTime IssuedAt { get; set; }
    }
}
