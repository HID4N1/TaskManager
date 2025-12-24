using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Enums;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Models;

/// <summary>
/// Task item entity for Kanban board
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    public Priority Priority { get; set; }

    public TaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public decimal? EstimatedHours { get; set; }

    public decimal? RealHours { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public string? AssignedUserId { get; set; }

    // Navigation properties
    public Project? Project { get; set; }
    public ApplicationUser? AssignedUser { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}

