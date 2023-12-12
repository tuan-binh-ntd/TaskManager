using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IProjectConfigurationRepository : IRepository<ProjectConfiguration>
{
    ProjectConfiguration Add(ProjectConfiguration projectConfiguration);
    void Update(ProjectConfiguration projectConfiguration);
    ProjectConfiguration GetByProjectId(Guid projectId);
}
