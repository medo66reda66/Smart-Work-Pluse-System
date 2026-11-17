using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Data.Configiration
{
    public class ApplicationuserConfig  : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<SmartWorkSpaseADL.Entities.Applicationuser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SmartWorkSpaseADL.Entities.Applicationuser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(u => u.Manager)
                .WithMany()
                .HasForeignKey(u => u.ManagerId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        }
    }
}