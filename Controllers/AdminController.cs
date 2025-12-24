using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Enums;

namespace TaskManager.Controllers;

/// <summary>
/// Admin controller for user management
/// Only ADMIN role can access
/// </summary>
[Authorize(Roles = "ADMIN")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // GET: Admin/Users
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    // GET: Admin/CreateUser
    public IActionResult CreateUser()
    {
        return View();
    }

    // POST: Admin/CreateUser
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(ViewModels.CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            Role = model.Role
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, model.Role.ToString());
            TempData["Success"] = $"User {model.Email} created successfully.";
            return RedirectToAction(nameof(Users));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // GET: Admin/EditUser/5
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Admin/EditUser/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(string id, Role role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Role = role;
        await _userManager.UpdateAsync(user);

        // Update role claims
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRoleAsync(user, role.ToString());

        return RedirectToAction(nameof(Users));
    }

    // POST: Admin/DeleteUser/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Prevent deleting yourself
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser?.Id == id)
        {
            TempData["Error"] = "You cannot delete your own account.";
            return RedirectToAction(nameof(Users));
        }

        await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(Users));
    }
}

