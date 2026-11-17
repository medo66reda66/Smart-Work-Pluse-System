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
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.HasOne(p => p.Leader)
                .WithMany(u => u.LedProjects)
                .HasForeignKey(p => p.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}