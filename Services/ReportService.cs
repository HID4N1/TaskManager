using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Services;

/// <summary>
/// Service for report generation
/// </summary>
public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<TaskStatus, int>> GetTaskCountByStatusAsync(Guid? projectId = null)
    {
        var query = _context.Tasks.AsQueryable();

        if (projectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == projectId.Value);
        }

        var tasks = await query.ToListAsync();

        var result = new Dictionary<TaskStatus, int>();
        foreach (TaskStatus status in Enum.GetValues<TaskStatus>())
        {
            result[status] = tasks.Count(t => t.Status == status);
        }

        return result;
    }

    public async Task<Report> GenerateReportAsync(ReportType type, string userId, string? period = null)
    {
        var report = new Report
        {
            Type = type,
            Period = period,
            GeneratedById = userId,
            GeneratedAt = DateTime.UtcNow
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task<List<Report>> GetReportsByUserAsync(string userId)
    {
        return await _context.Reports
            .Include(r => r.GeneratedBy)
            .Where(r => r.GeneratedById == userId)
            .OrderByDescending(r => r.GeneratedAt)
            .ToListAsync();
    }
}

