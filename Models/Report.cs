using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Enums;

namespace TaskManager.Models;

/// <summary>
/// Report entity for generated reports
/// </summary>
public class Report
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public ReportType Type { get; set; }

    [StringLength(100)]
    public string? Period { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string GeneratedById { get; set; } = string.Empty;

    // Navigation property
    public ApplicationUser? GeneratedBy { get; set; }
}

