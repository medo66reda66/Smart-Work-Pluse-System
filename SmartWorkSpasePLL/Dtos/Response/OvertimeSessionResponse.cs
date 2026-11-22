
using SmartWorkSpaseADL.Entities;

namespace SmartWorkSpasePLL.Dtos.Response
{
    public class OvertimeSessionResponse
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public int DurationMinutes { get; set; }

        public decimal RatePerMinute { get; set; }

        public decimal Amount { get; set; }

        public bool IsHoliday { get; set; }

        public OvertimeStatus Status { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
