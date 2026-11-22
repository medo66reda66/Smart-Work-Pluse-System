using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpaseADL.Entities
{
    public enum OvertimeStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class OvertimeSession
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }
        public Applicationuser Employee { get; set; } = null!;

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public string? ApprovedById { get; set; }
        public Applicationuser? ApprovedBy { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal RatePerMinute { get; set; }

        public decimal Amount { get; set; }
        public bool IsHoliday { get; set; }
        public OvertimeStatus Status { get; set; } = OvertimeStatus.Pending;
    }
}
