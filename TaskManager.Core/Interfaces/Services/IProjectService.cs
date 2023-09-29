using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Services
{
    /// <summary>
    /// Implementation business logic of project 
    /// </summary>
    public interface IProjectService
    {
        Task<Project> CreateProject(Project project);
    }
}
