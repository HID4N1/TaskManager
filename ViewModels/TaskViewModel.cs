using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.ViewModels;

/// <summary>
/// View model for task operations
/// </summary>
public class TaskViewModel
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    public Priority Priority { get; set; }

    public TaskStatus Status { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [Range(0, 1000)]
    public decimal? EstimatedHours { get; set; }

    [Range(0, 1000)]
    public decimal? RealHours { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public string? AssignedUserId { get; set; }

    public string? ProjectName { get; set; }
    public string? AssignedUserName { get; set; }
}

