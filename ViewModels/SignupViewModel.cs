using System.ComponentModel.DataAnnotations;
using TaskManager.Models.Enums;

namespace TaskManager.ViewModels;

/// <summary>
/// View model for user registration (signup)
/// Only MANAGER and MEMBER roles can be selected
/// </summary>
public class SignupViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Role")]
    public Role Role { get; set; } = Role.MEMBER;
}

