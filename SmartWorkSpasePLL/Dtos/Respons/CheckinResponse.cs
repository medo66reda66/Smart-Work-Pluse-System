using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Respons
{
    public class CheckinResponse
    {
        public bool CheckclosedCheckin { get; set; } = true;
        public bool CheckInOncePerDay { get; set; } = true ;

        public bool CheckFineBeforeEnter { get; set; } = true;

        public AttendanceRecord? AttendanceRecord { get; set; }

        public List<Card>? Cards { get; set; }

    }
}
