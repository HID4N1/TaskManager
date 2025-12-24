using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers;

/// <summary>
/// Comment controller for task comments
/// All authenticated users can add comments to tasks they can access
/// </summary>
[Authorize]
public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentController(
        ApplicationDbContext context,
        IProjectService projectService,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _projectService = projectService;
        _userManager = userManager;
    }

    // POST: Comment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid taskId, string content)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        if (string.IsNullOrWhiteSpace(content))
        {
            return RedirectToAction("Details", "Task", new { id = taskId });
        }

        var task = await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId);

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

        var comment = new Comment
        {
            Content = content,
            AuthorId = user.Id,
            TaskId = taskId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Task", new { id = taskId });
    }

    // POST: Comment/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        var comment = await _context.Comments
            .Include(c => c.Task)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        // Only author or ADMIN/MANAGER can delete
        if (comment.AuthorId != user.Id && user.Role != Models.Enums.Role.ADMIN && user.Role != Models.Enums.Role.MANAGER)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        var taskId = comment.TaskId;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Task", new { id = taskId });
    }
}

