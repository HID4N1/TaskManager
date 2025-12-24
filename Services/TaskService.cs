using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Services;

/// <summary>
/// Service for task business logic
/// </summary>
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId)
    {
        return await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(Guid id)
    {
        return await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
            .Include(t => t.Attachments)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        var task = await GetTaskByIdAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateTaskStatusAsync(Guid taskId, TaskStatus status)
    {
        var task = await GetTaskByIdAsync(taskId);
        if (task != null)
        {
            task.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    public Dictionary<TaskStatus, List<TaskItem>> GroupTasksByStatus(List<TaskItem> tasks)
    {
        var grouped = new Dictionary<TaskStatus, List<TaskItem>>();

        // Initialize all statuses
        foreach (TaskStatus status in Enum.GetValues<TaskStatus>())
        {
            grouped[status] = new List<TaskItem>();
        }

        // Group tasks by status
        foreach (var task in tasks)
        {
            grouped[task.Status].Add(task);
        }

        return grouped;
    }
}

