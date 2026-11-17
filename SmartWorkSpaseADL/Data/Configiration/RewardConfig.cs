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
    public class RewardConfig : IEntityTypeConfiguration<Reward>
    {
        public void Configure(EntityTypeBuilder<Reward> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.UsedAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.PayPalNumber)
                .HasMaxLength(50);

            builder.HasOne(r => r.ApplicationUser)
                .WithMany(e => e.Rewards)
                .HasForeignKey(r => r.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}