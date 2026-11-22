using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum Gender
    {
        Male,
        Female,
    }
    public class Applicationuser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public decimal BasicSalary { get; set; }
        public string Department { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Gavernate { get; set; }=string.Empty;
        public string City { get; set; }=string.Empty;
        public Gender gender { get; set; }
        public string ImageUrl { get; set; }=string.Empty;

        public string? ManagerId { get; set; } = string.Empty;
        public Applicationuser? Manager { get; set; }

        public ICollection<Notification> ReceivedNotifications { get; set; }
       = new List<Notification>();

        // Notifications related to this user (Employee)
        public ICollection<Notification> RelatedNotifications { get; set; }
            = new List<Notification>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();
        public ICollection<Fine> Fines { get; set; } = new List<Fine>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
        public ICollection<Project> LedProjects { get; set; } = new List<Project>();

        public ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
        public ICollection<OvertimeSession> Sessions { get; set; } = new List<OvertimeSession>();
        public ICollection<OvertimeSession> ApprovedSessions { get; set; } = new List<OvertimeSession>();
    }
}
