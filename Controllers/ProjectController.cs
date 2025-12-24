using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskManager.Services;
using TaskManager.ViewModels;

namespace TaskManager.Controllers;

/// <summary>
/// Project controller with role-based authorization
/// ADMIN and MANAGER can manage projects
/// MEMBER can only view assigned projects
/// </summary>
[Authorize]
public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectController(IProjectService projectService, UserManager<ApplicationUser> userManager)
    {
        _projectService = projectService;
        _userManager = userManager;
    }

    // GET: Project
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var projects = await _projectService.GetProjectsByUserAsync(user.Id);
        return View(projects);
    }

    // GET: Project/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, id);
        if (!canAccess)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // GET: Project/Create
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Create()
    {
        // Get all managers for dropdown
        var managers = await _userManager.Users
            .Where(u => u.Role == Role.MANAGER || u.Role == Role.ADMIN)
            .OrderBy(u => u.Email)
            .ToListAsync();
        ViewBag.Managers = managers;
        return View();
    }

    // POST: Project/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Create(ProjectViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            // Reload managers for dropdown
            var managers = await _userManager.Users
                .Where(u => u.Role == Role.MANAGER || u.Role == Role.ADMIN)
                .OrderBy(u => u.Email)
                .ToListAsync();
            ViewBag.Managers = managers;
            return View(viewModel);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var project = new Project
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            StartDate = viewModel.StartDate,
            EndDate = viewModel.EndDate,
            Status = viewModel.Status,
            CreatorId = user.Id,
            ManagerId = string.IsNullOrEmpty(viewModel.ManagerId) ? null : viewModel.ManagerId
        };

        await _projectService.CreateProjectAsync(project);
        TempData["Success"] = "Project created successfully!";
        return RedirectToAction(nameof(Index));
    }

    // GET: Project/Edit/5
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        // Get all managers for dropdown
        var managers = await _userManager.Users
            .Where(u => u.Role == Role.MANAGER || u.Role == Role.ADMIN)
            .OrderBy(u => u.Email)
            .ToListAsync();
        ViewBag.Managers = managers;

        var viewModel = new ProjectViewModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            ManagerId = project.ManagerId,
            ManagerName = project.Manager?.Email
        };

        return View(viewModel);
    }

    // POST: Project/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Edit(Guid id, ProjectViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        project.Name = viewModel.Name;
        project.Description = viewModel.Description;
        project.StartDate = viewModel.StartDate;
        project.EndDate = viewModel.EndDate;
        project.Status = viewModel.Status;
        project.ManagerId = string.IsNullOrEmpty(viewModel.ManagerId) ? null : viewModel.ManagerId;

        await _projectService.UpdateProjectAsync(project);
        TempData["Success"] = "Project updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    // GET: Project/Delete/5
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // POST: Project/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _projectService.DeleteProjectAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

