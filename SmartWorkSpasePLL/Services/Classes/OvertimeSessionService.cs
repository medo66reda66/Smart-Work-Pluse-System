
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Repositories.Interfaces;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Dtos.Respons;
using SmartWorkSpasePLL.Services.Interfaces;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class OvertimeSessionService : IOvertimeSessionService
    {
        private readonly IRepository<OvertimeSession> _overtimeRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly ICompanySettingService _companySettingService;

        public OvertimeSessionService(
            IRepository<OvertimeSession> overtimeRepository,
            IRepository<Project> projectRepository,
            ICompanySettingService companySettingService)
        {
            _overtimeRepository = overtimeRepository;
            _projectRepository = projectRepository;
            _companySettingService = companySettingService;
        }

        // 1. Create Overtime Session
        public async Task<OvertimeSessionResponse> CreateOvertimeSessionAsync(
            string employeeId,
            CreateOvertimeSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                throw new ArgumentException("Employee ID is required.");
            }

            if (request == null)
            {
                throw new ArgumentException("Request data is required.");
            }

            var companySettings =
                await _companySettingService.GetCompanySettingAsync();

            if (companySettings == null || companySettings.Count == 0)
            {
                throw new InvalidOperationException(
                    "Company settings have not been configured.");
            }

            var settings = companySettings[0];

            if (!settings.EnableOvertime)
            {
                throw new InvalidOperationException(
                    "Overtime is currently disabled.");
            }

            var project = await _projectRepository.GetByIdAsync(
                p => p.Id == request.ProjectId);

            if (project == null)
            {
                throw new InvalidOperationException(
                    "The selected project does not exist.");
            }

            if (request.StartedAt == default ||
                request.EndedAt == default)
            {
                throw new ArgumentException(
                    "Start time and end time are required.");
            }

            if (request.EndedAt <= request.StartedAt)
            {
                throw new ArgumentException(
                    "End time must be after start time.");
            }

            if (request.StartedAt.TimeOfDay < settings.WorkEndTime)
            {
                throw new InvalidOperationException(
                    $"Overtime must start after official working hours ({settings.WorkEndTime}).");
            }

            int durationMinutes = (int)(
                request.EndedAt - request.StartedAt
            ).TotalMinutes;

            if (durationMinutes <= 0)
            {
                throw new ArgumentException(
                    "Overtime duration must be at least one minute.");
            }

            // Check overlapping sessions
            var overlappingSessions = await _overtimeRepository.GetAllAsync(
                s => s.EmployeeId == employeeId
                && s.Status != OvertimeStatus.Rejected
                && s.StartedAt < request.EndedAt
                && (s.EndedAt == null ||
                    s.EndedAt > request.StartedAt)
            );

            if (overlappingSessions.Any())
            {
                throw new InvalidOperationException(
                    "This overtime session overlaps with an existing session.");
            }

            // Check daily limit
            var dailySessions = await _overtimeRepository.GetAllAsync(
                s => s.EmployeeId == employeeId
                && s.StartedAt.Date == request.StartedAt.Date
                && (s.Status == OvertimeStatus.Pending ||
                    s.Status == OvertimeStatus.Approved)
            );

            int totalDailyMinutes = dailySessions.Sum(
                s => s.DurationMinutes);

            if (totalDailyMinutes + durationMinutes >
                settings.MaxOvertimeMinutes)
            {
                throw new InvalidOperationException(
                    $"Daily overtime limit is {settings.MaxOvertimeMinutes} minutes. " +
                    $"You already have {totalDailyMinutes} minutes recorded.");
            }

            bool isHoliday =
                request.StartedAt.DayOfWeek == DayOfWeek.Friday ||
                request.StartedAt.DayOfWeek == DayOfWeek.Saturday;

            decimal multiplier = isHoliday
                ? settings.HolidayOvertimeMultiplier
                : settings.OvertimeMultiplier;

            decimal ratePerMinute =
                (settings.OvertimeBaseRatePerHour / 60m) * multiplier;

            decimal amount = durationMinutes * ratePerMinute;

            var overtimeSession = new OvertimeSession
            {
                EmployeeId = employeeId,
                ProjectId = request.ProjectId,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                Reason = request.Reason,
                DurationMinutes = durationMinutes,
                RatePerMinute = ratePerMinute,
                Amount = amount,
                IsHoliday = isHoliday,
                Status = OvertimeStatus.Pending
            };

            _overtimeRepository.Add(overtimeSession);

            await _overtimeRepository.SaveChangesAsync();

            return MapToResponse(overtimeSession, project.Name);
        }

        // 2. Get My Overtime Sessions
        public async Task<List<OvertimeSessionResponse>>
            GetMyOvertimeSessionsAsync(string employeeId)
        {
            ValidateEmployeeId(employeeId);

            var sessions = await _overtimeRepository.GetAllAsync(
                s => s.EmployeeId == employeeId,
                query => query.OrderByDescending(s => s.StartedAt)
            );

            var response = new List<OvertimeSessionResponse>();

            foreach (var session in sessions)
            {
                var project = await _projectRepository.GetByIdAsync(
                    p => p.Id == session.ProjectId);

                response.Add(
                    MapToResponse(
                        session,
                        project?.Name ?? "Unknown Project"
                    )
                );
            }

            return response;
        }

        // 3. Get Overtime Session By ID
        public async Task<OvertimeSessionResponse>
            GetOvertimeSessionByIdAsync(int id, string employeeId)
        {
            ValidateEmployeeId(employeeId);
            ValidateSessionId(id);

            var session = await _overtimeRepository.GetByIdAsync(
                s => s.Id == id && s.EmployeeId == employeeId
            );

            if (session == null)
            {
                throw new KeyNotFoundException(
                    "Overtime session not found.");
            }

            var project = await _projectRepository.GetByIdAsync(
                p => p.Id == session.ProjectId);

            return MapToResponse(
                session,
                project?.Name ?? "Unknown Project"
            );
        }

        // 4. Get My Overtime Sessions Summary
        public async Task<OvertimeSessionSummaryResponse>
            GetMyOvertimeSessionsSummaryAsync(string employeeId)
        {
            ValidateEmployeeId(employeeId);

            var sessions = await _overtimeRepository.GetAllAsync(
                s => s.EmployeeId == employeeId
            );

            var companySettings =
                await _companySettingService.GetCompanySettingAsync();

            if (companySettings == null || companySettings.Count == 0)
            {
                throw new InvalidOperationException(
                    "Company settings have not been configured.");
            }

            var settings = companySettings[0];

            var today = DateTime.Today;

            var todaySessions = sessions
                .Where(s =>
                    s.StartedAt.Date == today &&
                    (s.Status == OvertimeStatus.Pending ||
                     s.Status == OvertimeStatus.Approved))
                .ToList();

            int todayDurationMinutes = todaySessions.Sum(
                s => s.DurationMinutes);

            return new OvertimeSessionSummaryResponse
            {
                TotalSessions = sessions.Count,

                PendingSessions = sessions.Count(
                    s => s.Status == OvertimeStatus.Pending),

                ApprovedSessions = sessions.Count(
                    s => s.Status == OvertimeStatus.Approved),

                RejectedSessions = sessions.Count(
                    s => s.Status == OvertimeStatus.Rejected),

                TotalDurationMinutes = sessions.Sum(
                    s => s.DurationMinutes),

                TodayDurationMinutes = todayDurationMinutes,

                DailyLimitMinutes = settings.MaxOvertimeMinutes,

                RemainingTodayMinutes = Math.Max(
                    0,
                    settings.MaxOvertimeMinutes - todayDurationMinutes),

                ApprovedAmount = sessions
                    .Where(s => s.Status == OvertimeStatus.Approved)
                    .Sum(s => s.Amount)
            };
        }

        // 5. Approve Overtime Session
        public async Task<OvertimeSessionResponse>
            ApproveOvertimeSessionAsync(int id, string approverId)
        {
            ValidateSessionId(id);
            ValidateApproverId(approverId);

            var session = await _overtimeRepository.GetByIdAsync(
                s => s.Id == id
            );

            if (session == null)
            {
                throw new KeyNotFoundException(
                    "Overtime session not found.");
            }

            if (session.Status != OvertimeStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending overtime sessions can be approved.");
            }

            session.Status = OvertimeStatus.Approved;
            session.ApprovedById = approverId;

            _overtimeRepository.Update(session);

            await _overtimeRepository.SaveChangesAsync();

            var project = await _projectRepository.GetByIdAsync(
                p => p.Id == session.ProjectId);

            return MapToResponse(
                session,
                project?.Name ?? "Unknown Project"
            );
        }

        // 6. Reject Overtime Session
        public async Task<OvertimeSessionResponse>
            RejectOvertimeSessionAsync(int id, string approverId)
        {
            ValidateSessionId(id);
            ValidateApproverId(approverId);

            var session = await _overtimeRepository.GetByIdAsync(
                s => s.Id == id
            );

            if (session == null)
            {
                throw new KeyNotFoundException(
                    "Overtime session not found.");
            }

            if (session.Status != OvertimeStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Only pending overtime sessions can be rejected.");
            }

            session.Status = OvertimeStatus.Rejected;

            _overtimeRepository.Update(session);

            await _overtimeRepository.SaveChangesAsync();

            var project = await _projectRepository.GetByIdAsync(
                p => p.Id == session.ProjectId);

            return MapToResponse(
                session,
                project?.Name ?? "Unknown Project"
            );
        }

        // Helper Methods

        private static void ValidateEmployeeId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                throw new ArgumentException(
                    "Employee ID is required.");
            }
        }

        private static void ValidateApproverId(string approverId)
        {
            if (string.IsNullOrWhiteSpace(approverId))
            {
                throw new ArgumentException(
                    "Approver ID is required.");
            }
        }

        private static void ValidateSessionId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Invalid overtime session ID.");
            }
        }

        private static OvertimeSessionResponse MapToResponse(
            OvertimeSession session,
            string projectName)
        {
            return new OvertimeSessionResponse
            {
                Id = session.Id,
                ProjectId = session.ProjectId,
                ProjectName = projectName,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                DurationMinutes = session.DurationMinutes,
                RatePerMinute = session.RatePerMinute,
                Amount = session.Amount,
                IsHoliday = session.IsHoliday,
                Status = session.Status,
                Reason = session.Reason
            };
        }
    }
}
