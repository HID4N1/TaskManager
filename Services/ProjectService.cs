using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Enums;

namespace TaskManager.Services;

/// <summary>
/// Service for project business logic
/// </summary>
public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Creator)
            .Include(p => p.Manager)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }

    public async Task<List<Project>> GetProjectsByUserAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return new List<Project>();

        // ADMIN and MANAGER can see all projects
        if (user.Role == Role.ADMIN || user.Role == Role.MANAGER)
        {
            return await GetAllProjectsAsync();
        }

        // MEMBER can only see projects where they have assigned tasks
        return await _context.Projects
            .Include(p => p.Creator)
            .Include(p => p.Manager)
            .Where(p => p.Tasks.Any(t => t.AssignedUserId == userId))
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Creator)
            .Include(p => p.Manager)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task UpdateProjectAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Guid id)
    {
        var project = await GetProjectByIdAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CanUserAccessProjectAsync(string userId, Guid projectId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        // ADMIN can access all projects
        if (user.Role == Role.ADMIN)
        {
            return true;
        }

        // MANAGER can access all projects OR projects assigned to them
        if (user.Role == Role.MANAGER)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project != null && project.ManagerId == userId)
            {
                return true; // Manager is assigned to this project
            }
            return true; // MANAGER can access all projects
        }

        // MEMBER can only access projects where they have assigned tasks
        return await _context.Tasks
            .AnyAsync(t => t.ProjectId == projectId && t.AssignedUserId == userId);
    }
}

