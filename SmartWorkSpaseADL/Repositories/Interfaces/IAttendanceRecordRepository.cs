using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Repositories.Interfaces
{
    public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
    {
        Task<List<AttendanceRecord>> GetLast30RecordsAsync(string userId);
    }
}
