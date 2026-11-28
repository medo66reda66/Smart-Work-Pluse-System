using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Dtos.Respons
{
    public class CheckOutResponse
    {
        public bool CheckClosedCheckOut { get; set; } = true;
         public bool   CheckOutOncePerDay { get; set; } = true ;
         public AttendanceRecord  CreateAttendanceRecordChecKOut {  get; set; }
    }
}
