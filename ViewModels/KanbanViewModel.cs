using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.ViewModels;

/// <summary>
/// View model for Kanban board display
/// </summary>
public class KanbanViewModel
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    // Tasks grouped by status
    public Dictionary<TaskStatus, List<TaskItem>> TasksByStatus { get; set; } = new();
}

