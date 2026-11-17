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
    public class CardConfig : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Color)
                .HasMaxLength(20);

            builder.Property(c => c.Source)
                .HasMaxLength(50);

            builder.Property(c => c.Status)
                .HasMaxLength(30);

            builder.HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.AttendanceRecord)
                .WithMany(a => a.Cards)
                .HasForeignKey(c => c.AttendanceRecordId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}