using Microsoft.AspNetCore.Identity;
using TaskManager.Models.Enums;

namespace TaskManager.Models;

/// <summary>
/// Application user extending IdentityUser with role enum
/// </summary>
public class ApplicationUser : IdentityUser
{
    public Role Role { get; set; }
}

