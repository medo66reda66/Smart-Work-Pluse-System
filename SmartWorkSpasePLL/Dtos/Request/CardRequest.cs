using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Request
{
    public class CardRequest
    {
        public string ApplicationUserId { get; set; } = string.Empty;
        public int AttendanceRecordId { get; set; }
        public CardColor Color { get; set; }

        public string Source { get; set; } = string.Empty;

        public CardStatus Status { get; set; }
        public DateTime IssuedAt { get; set; }= DateTime.Now;
    }
}
