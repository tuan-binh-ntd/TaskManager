namespace TaskManager.Core.Interfaces.Repositories;

public interface IProjectConfigurationRepository
{
    Task<ProjectConfiguration?> GetProjectConfigurationByProjectIdAsync(Guid projectId);
    Task<Guid?> GetDefaultAssigneeIdByProjectIdAsync(Guid projectId);
    Task UpdateDefaultAssigneeAsync(Guid projectId, Guid? defaultAssigneeId);
    void Update(ProjectConfiguration projectConfiguration);
    void Insert(ProjectConfiguration projectConfiguration);
    void Remove(ProjectConfiguration projectConfiguration);
    Task<ProjectConfiguration?> GetByIdAsync(Guid id);
}
