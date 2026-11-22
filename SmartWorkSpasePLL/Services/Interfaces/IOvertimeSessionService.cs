
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Dtos.Respons;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface IOvertimeSessionService
    {
        Task<OvertimeSessionResponse> CreateOvertimeSessionAsync(
            string employeeId,
            CreateOvertimeSessionRequest request);

        Task<List<OvertimeSessionResponse>> GetMyOvertimeSessionsAsync(
            string employeeId);

        Task<OvertimeSessionResponse> GetOvertimeSessionByIdAsync(
            int id,
            string employeeId);

        Task<OvertimeSessionSummaryResponse> GetMyOvertimeSessionsSummaryAsync(
            string employeeId);

        Task<OvertimeSessionResponse> ApproveOvertimeSessionAsync(
            int id,
            string approverId);

        Task<OvertimeSessionResponse> RejectOvertimeSessionAsync(
            int id,
            string approverId);
    }
}
