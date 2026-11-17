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
    public class AttendanceRecordConfig : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.HasKey(ar => ar.Id);

            builder.Property(ar => ar.Status)
                .HasMaxLength(30);

            builder.HasOne(ar => ar.Applicationuser)
                .WithMany(u => u.AttendanceRecords)
                .HasForeignKey(ar => ar.ApplicationuserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
