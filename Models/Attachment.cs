using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

/// <summary>
/// Attachment entity for task files
/// </summary>
public class Attachment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(500)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string FilePath { get; set; } = string.Empty;

    public long Size { get; set; }

    [StringLength(100)]
    public string? Type { get; set; }

    [Required]
    public Guid TaskId { get; set; }

    // Navigation property
    public TaskItem? Task { get; set; }
}

