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
    public class ProjectTaskConfig : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(pt => pt.Id);
            builder.Property(pt => pt.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pt => pt.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(pt => pt.PercentComplete)
                .IsRequired();

            builder.Property(pt => pt.Notes)
                .HasMaxLength(500);

            builder.Property(pt => pt.DueDate)
                .IsRequired(false);

            builder.HasKey(pt => pt.Id);

            builder.HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pt => pt.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(pt => pt.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
