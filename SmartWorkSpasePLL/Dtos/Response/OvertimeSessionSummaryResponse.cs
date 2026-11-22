
namespace SmartWorkSpasePLL.Dtos.Response
{
    public class OvertimeSessionSummaryResponse
    {
        public int TotalSessions { get; set; }

        public int PendingSessions { get; set; }

        public int ApprovedSessions { get; set; }

        public int RejectedSessions { get; set; }

        public int TotalDurationMinutes { get; set; }

        public int TodayDurationMinutes { get; set; }

        public int DailyLimitMinutes { get; set; }

        public int RemainingTodayMinutes { get; set; }

        public decimal ApprovedAmount { get; set; }
    }
}
