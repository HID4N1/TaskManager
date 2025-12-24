using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Services;

/// <summary>
/// Service interface for report operations
/// </summary>
public interface IReportService
{
    Task<Dictionary<TaskStatus, int>> GetTaskCountByStatusAsync(Guid? projectId = null);
    Task<Report> GenerateReportAsync(ReportType type, string userId, string? period = null);
    Task<List<Report>> GetReportsByUserAsync(string userId);
}

