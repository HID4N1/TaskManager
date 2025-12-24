using TaskManager.Models;

namespace TaskManager.Services;

/// <summary>
/// Service interface for project operations
/// </summary>
public interface IProjectService
{
    Task<List<Project>> GetAllProjectsAsync();
    Task<List<Project>> GetProjectsByUserAsync(string userId);
    Task<Project?> GetProjectByIdAsync(Guid id);
    Task<Project> CreateProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(Guid id);
    Task<bool> CanUserAccessProjectAsync(string userId, Guid projectId);
}

