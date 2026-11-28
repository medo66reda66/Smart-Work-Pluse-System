using Microsoft.EntityFrameworkCore;
using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;

namespace SmartWorkSpaseADL.Repositories.Classes
{
    public class AttendanceRecordRepository :Repository<AttendanceRecord>, IAttendanceRecordRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRecordRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<List<AttendanceRecord>> GetLast30RecordsAsync(string userId)
        {
            return await _context.AttendanceRecords
                .Where(e => e.ApplicationuserId == userId && e.IsDeleted != true)
                .OrderByDescending(e => e.Id)
                .Take(30)
                .ToListAsync();
        }
    }
}
