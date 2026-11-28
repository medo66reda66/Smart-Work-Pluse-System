using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Respons
{
    public class CardResponseWithAttendanceRecord
    {
        public CardColor Color { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
