using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum ProjectStatus
    {
        NotStarted,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LeaderId { get; set; } = string.Empty;
        public Applicationuser Leader { get; set; } = null!;
        public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }

        public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();

        public ICollection<OvertimeSession> OvertimeSessions { get; set; } = new List<OvertimeSession>();
    }
}
