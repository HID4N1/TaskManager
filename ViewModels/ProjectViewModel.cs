using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Enums;

namespace TaskManager.ViewModels;

/// <summary>
/// View model for project operations
/// </summary>
public class ProjectViewModel
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public ProjectStatus Status { get; set; }

    /// <summary>
    /// Manager assigned to this project (optional)
    /// </summary>
    public string? ManagerId { get; set; }

    public string? ManagerName { get; set; }
}

