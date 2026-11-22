using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWorkSpasePLL.Dtos.Request
{
    public class CreateOvertimeSessionRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        [Required]
        public DateTime StartedAt { get; set; }

        [Required]
        public DateTime EndedAt { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Reason { get; set; } = string.Empty;
    }
}
