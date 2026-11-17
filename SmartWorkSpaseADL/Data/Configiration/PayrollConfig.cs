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
    public class PayrollConfig : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.BaseSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.OvertimeTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.RewardsTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.FinesTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.NetSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(p => p.Applicationuser)
                .WithMany(e => e.Payrolls)
                .HasForeignKey(p => p.ApplicationuserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}