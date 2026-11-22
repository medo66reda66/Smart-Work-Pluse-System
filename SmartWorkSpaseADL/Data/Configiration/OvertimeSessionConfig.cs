using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Data.Configiration
{
    public class OvertimeSessionConfig : IEntityTypeConfiguration<OvertimeSession>
    {
        public void Configure(EntityTypeBuilder<OvertimeSession> builder)
        {
            builder.HasKey(os => os.Id);

            builder.Property(os => os.DurationMinutes)
                .IsRequired();

            builder.Property(os => os.RatePerMinute)
                .IsRequired()
                .HasPrecision(18, 4);
            builder.Property(os => os.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            builder.Property(os => os.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(os => os.Employee)
                .WithMany(e => e.Sessions)
                .HasForeignKey(os => os.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(os => os.Project)
                .WithMany(p => p.OvertimeSessions)
                .HasForeignKey(os => os.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(os => os.ApprovedBy)
                .WithMany(a => a.ApprovedSessions)
                .HasForeignKey(os => os.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
