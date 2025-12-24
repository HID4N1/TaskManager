using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskManager.Services;
using TaskManager.ViewModels;
using TaskStatus = TaskManager.Models.Enums.TaskStatus;

namespace TaskManager.Controllers;

/// <summary>
/// Task controller with role-based authorization
/// ADMIN and MANAGER can manage all tasks
/// MEMBER can view and update own tasks
/// </summary>
[Authorize]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public TaskController(
        ITaskService taskService,
        IProjectService projectService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _taskService = taskService;
        _projectService = projectService;
        _userManager = userManager;
        _context = context;
    }

    // GET: Task
    public async Task<IActionResult> Index(Guid? projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        List<TaskItem> tasks;

        if (projectId.HasValue)
        {
            var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, projectId.Value);
            if (!canAccess)
            {
                return RedirectToAction("AccessDenied", "Auth");
            }

            tasks = await _taskService.GetTasksByProjectAsync(projectId.Value);
        }
        else
        {
            // Show all tasks user can access
            var projects = await _projectService.GetProjectsByUserAsync(user.Id);
            var projectIds = projects.Select(p => p.Id).ToList();
            tasks = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Where(t => projectIds.Contains(t.ProjectId))
                .ToListAsync();
        }

        return View(tasks);
    }

    // GET: Task/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Check access
        var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, task.ProjectId);
        if (!canAccess)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        // MEMBER can only view own tasks
        if (user.Role == Role.MEMBER && task.AssignedUserId != user.Id)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        return View(task);
    }

    // GET: Task/Kanban/5
    public async Task<IActionResult> Kanban(Guid projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, projectId);
        if (!canAccess)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        var tasks = await _taskService.GetTasksByProjectAsync(projectId);
        var groupedTasks = _taskService.GroupTasksByStatus(tasks);

        var viewModel = new KanbanViewModel
        {
            ProjectId = projectId,
            ProjectName = project.Name,
            TasksByStatus = groupedTasks
        };

        return View(viewModel);
    }

    // GET: Task/Create
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Create(Guid? projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var projects = await _projectService.GetProjectsByUserAsync(user.Id);
        ViewBag.Projects = projects;
        // Only show MEMBER role users for task assignment
        ViewBag.Users = await _context.Users
            .Where(u => u.Role == Role.MEMBER)
            .OrderBy(u => u.Email)
            .ToListAsync();

        var viewModel = new TaskViewModel
        {
            ProjectId = projectId ?? Guid.Empty
        };

        return View(viewModel);
    }

    // POST: Task/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Create(TaskViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var projects = await _projectService.GetProjectsByUserAsync(user.Id);
            ViewBag.Projects = projects;
            // Only show MEMBER role users for task assignment
            ViewBag.Users = await _context.Users
                .Where(u => u.Role == Role.MEMBER)
                .OrderBy(u => u.Email)
                .ToListAsync();
            return View(viewModel);
        }

        var task = new TaskItem
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
            Priority = viewModel.Priority,
            Status = viewModel.Status,
            DueDate = viewModel.DueDate,
            EstimatedHours = viewModel.EstimatedHours,
            RealHours = viewModel.RealHours,
            ProjectId = viewModel.ProjectId,
            AssignedUserId = viewModel.AssignedUserId
        };

        await _taskService.CreateTaskAsync(task);
        return RedirectToAction(nameof(Kanban), new { projectId = viewModel.ProjectId });
    }

    // GET: Task/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Check access
        var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, task.ProjectId);
        if (!canAccess)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        // MEMBER can only edit own tasks
        if (user.Role == Role.MEMBER && task.AssignedUserId != user.Id)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var projects = await _projectService.GetProjectsByUserAsync(user.Id);
        ViewBag.Projects = projects;
        // Only show MEMBER role users for task assignment
        ViewBag.Users = await _context.Users
            .Where(u => u.Role == Role.MEMBER)
            .OrderBy(u => u.Email)
            .ToListAsync();

        var viewModel = new TaskViewModel
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            DueDate = task.DueDate,
            EstimatedHours = task.EstimatedHours,
            RealHours = task.RealHours,
            ProjectId = task.ProjectId,
            AssignedUserId = task.AssignedUserId
        };

        return View(viewModel);
    }

    // POST: Task/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TaskViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Check access
        var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, task.ProjectId);
        if (!canAccess)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        // MEMBER can only edit own tasks, and cannot change assignment
        if (user.Role == Role.MEMBER)
        {
            if (task.AssignedUserId != user.Id)
            {
                return RedirectToAction("AccessDenied", "Auth");
            }
            // MEMBER cannot change assignment
            viewModel.AssignedUserId = task.AssignedUserId;
        }

        if (!ModelState.IsValid)
        {
            var projects = await _projectService.GetProjectsByUserAsync(user.Id);
            ViewBag.Projects = projects;
            // Only show MEMBER role users for task assignment
            ViewBag.Users = await _context.Users
                .Where(u => u.Role == Role.MEMBER)
                .OrderBy(u => u.Email)
                .ToListAsync();
            return View(viewModel);
        }

        task.Title = viewModel.Title;
        task.Description = viewModel.Description;
        task.Priority = viewModel.Priority;
        task.Status = viewModel.Status;
        task.DueDate = viewModel.DueDate;
        task.EstimatedHours = viewModel.EstimatedHours;
        task.RealHours = viewModel.RealHours;
        task.AssignedUserId = viewModel.AssignedUserId;

        await _taskService.UpdateTaskAsync(task);
        return RedirectToAction(nameof(Kanban), new { projectId = task.ProjectId });
    }

    // POST: Task/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(Guid taskId, TaskStatus status)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var task = await _taskService.GetTaskByIdAsync(taskId);
        if (task == null)
        {
            return NotFound();
        }

        // Check access
        var canAccess = await _projectService.CanUserAccessProjectAsync(user.Id, task.ProjectId);
        if (!canAccess)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        // MEMBER can only update own tasks
        if (user.Role == Role.MEMBER && task.AssignedUserId != user.Id)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        await _taskService.UpdateTaskStatusAsync(taskId, status);
        return RedirectToAction(nameof(Kanban), new { projectId = task.ProjectId });
    }

    // GET: Task/Delete/5
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    // POST: Task/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        var projectId = task.ProjectId;
        await _taskService.DeleteTaskAsync(id);
        return RedirectToAction(nameof(Kanban), new { projectId });
    }
}

