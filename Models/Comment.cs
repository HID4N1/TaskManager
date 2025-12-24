using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models;

/// <summary>
/// Comment entity for task discussions
/// </summary>
public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string AuthorId { get; set; } = string.Empty;

    [Required]
    public Guid TaskId { get; set; }

    // Navigation properties
    public ApplicationUser? Author { get; set; }
    public TaskItem? Task { get; set; }
}

