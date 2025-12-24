using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels;

/// <summary>
/// View model for user login
/// </summary>
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

