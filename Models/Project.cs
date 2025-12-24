using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Enums;

namespace TaskManager.Models;

/// <summary>
/// Project entity representing a Scrum project
/// </summary>
public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public ProjectStatus Status { get; set; }

    [Required]
    public string CreatorId { get; set; } = string.Empty;

    /// <summary>
    /// Manager assigned to this project (optional)
    /// </summary>
    public string? ManagerId { get; set; }

    // Navigation properties
    public ApplicationUser? Creator { get; set; }
    public ApplicationUser? Manager { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}

