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
    public class FineConfig : IEntityTypeConfiguration<Fine>
    {
        public void Configure(EntityTypeBuilder<Fine> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(f => f.ApplicationUser)
                .WithMany(e => e.Fines)
                .HasForeignKey(f => f.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
