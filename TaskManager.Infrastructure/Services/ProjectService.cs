using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IBacklogRepository _backlogRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(
            IBacklogRepository backlogRepository,
            IProjectRepository projectRepository
            )
        {
            _backlogRepository = backlogRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Project> CreateProject(Project project)
        {
            var newProject = _projectRepository.Add(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
            var backlog = new Backlog()
            {
                Name = project.Name,
                ProjectId = newProject.Id,
            };
            _backlogRepository.Add(backlog);
            await _backlogRepository.UnitOfWork.SaveChangesAsync();

            return newProject;
        }
    }
}
