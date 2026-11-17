using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Entities.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Data.Context
{
    public class AppDbContext :IdentityDbContext<Applicationuser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Fine> Fines { get; set; }

        public DbSet<Payroll> Payrolls { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }

        public DbSet<OvertimeSession> OvertimeSessions { get; set; }

        public DbSet<CompanySetting> CompanySettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}
