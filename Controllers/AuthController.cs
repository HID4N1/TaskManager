using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskManager.ViewModels;

namespace TaskManager.Controllers;

/// <summary>
/// Authentication controller for login/logout
/// </summary>
public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Signup(SignupViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Only allow MANAGER or MEMBER roles for self-registration
        if (model.Role != Role.MANAGER && model.Role != Role.MEMBER)
        {
            ModelState.AddModelError(nameof(model.Role), "Only MANAGER and MEMBER roles can be selected for registration. ADMIN accounts must be created by existing administrators.");
            return View(model);
        }

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(model.Email), "A user with this email already exists. Please use a different email or try logging in.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            Role = model.Role
        };

        // Ensure the role exists before creating user
        var roleName = model.Role.ToString();
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            // Create the role if it doesn't exist
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            // Add user to role
            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                // If role assignment fails, log errors but still allow signup
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, $"Warning: Could not assign role - {error.Description}");
                }
            }

            // Automatically sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            TempData["Success"] = $"Account created successfully! Welcome, {model.Email}. You are logged in as {model.Role}.";
            return RedirectToAction("Index", "Project");
        }

        // Add specific error messages for common issues
        foreach (var error in result.Errors)
        {
            if (error.Code == "DuplicateUserName" || error.Code == "DuplicateEmail")
            {
                ModelState.AddModelError(nameof(model.Email), error.Description);
            }
            else if (error.Code.Contains("Password"))
            {
                ModelState.AddModelError(nameof(model.Password), error.Description);
            }
            else
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToLocal(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Index", "Project");
    }
}

