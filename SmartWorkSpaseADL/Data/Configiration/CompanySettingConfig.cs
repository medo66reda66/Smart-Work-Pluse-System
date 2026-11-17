using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Entities.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Data.Configiration
{
    public class CompanySettingConfig : IEntityTypeConfiguration<CompanySetting>
    {
        public void Configure(EntityTypeBuilder<CompanySetting> builder)
        {
            builder.HasKey(cs => cs.Id);

            builder.Property(cs => cs.WorkStartTime)
                .IsRequired();

            builder.Property(cs => cs.WorkEndTime)
                .IsRequired();

            builder.Property(cs => cs.BlueWindowMinutes)
                .IsRequired();

         builder.Property(cs => cs.GreenCardsPerEarlyArrival)
                .IsRequired();


            builder.Property(cs => cs.GreenForReward)
                .IsRequired();

            builder.Property(cs => cs.BlueForFine)
                    .IsRequired();

            builder.Property(cs => cs.BlackForFine)
                .IsRequired();

            builder.Property(cs => cs.RewardAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cs => cs.BlueFineAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cs => cs.BlackFineAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cs => cs.RequireFinePaymentBeforeCheckIn)
                .IsRequired();

            builder.Property(cs => cs.OvertimeMultiplier)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cs => cs.HolidayOvertimeMultiplier)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cs => cs.OvertimeBaseRatePerHour)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(cs => cs.WorkingHoursPerDay)
                .IsRequired();

            builder.Property(cs => cs.MaxOvertimeMinutes)
                .IsRequired();

        }
    }
}
