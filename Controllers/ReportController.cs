using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskManager.Services;

namespace TaskManager.Controllers;

/// <summary>
/// Report controller for generating reports
/// ADMIN and MANAGER can generate reports
/// </summary>
[Authorize(Roles = "ADMIN,MANAGER")]
public class ReportController : Controller
{
    private readonly IReportService _reportService;
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReportController(
        IReportService reportService,
        ITaskService taskService,
        IProjectService projectService,
        UserManager<ApplicationUser> userManager)
    {
        _reportService = reportService;
        _taskService = taskService;
        _projectService = projectService;
        _userManager = userManager;
    }

    // GET: Report
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var reports = await _reportService.GetReportsByUserAsync(user.Id);
        return View(reports);
    }

    // GET: Report/TaskByStatus
    public async Task<IActionResult> TaskByStatus(Guid? projectId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var taskCounts = await _reportService.GetTaskCountByStatusAsync(projectId);
        
        ViewBag.ProjectId = projectId;
        if (projectId.HasValue)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId.Value);
            ViewBag.ProjectName = project?.Name;
        }

        // Generate report record
        var period = projectId.HasValue ? $"Project: {ViewBag.ProjectName}" : "All Projects";
        await _reportService.GenerateReportAsync(ReportType.TASK_BY_STATUS, user.Id, period);

        return View(taskCounts);
    }
}

