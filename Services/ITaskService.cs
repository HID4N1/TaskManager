using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Services;

/// <summary>
/// Service interface for task operations
/// </summary>
public interface ITaskService
{
    Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId);
    Task<TaskItem?> GetTaskByIdAsync(Guid id);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(Guid id);
    Task UpdateTaskStatusAsync(Guid taskId, TaskStatus status);
    Dictionary<TaskStatus, List<TaskItem>> GroupTasksByStatus(List<TaskItem> tasks);
}

