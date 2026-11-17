using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
    public class ProjectTask
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public string? AssignedToId { get; set; } = string.Empty;
        public Applicationuser AssignedTo { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = TaskStatus.ToDo.ToString();
        public int PercentComplete { get; set; }
        public string? Notes { get; set; }
        public DateTime? DueDate { get; set; }

    
    }
}
