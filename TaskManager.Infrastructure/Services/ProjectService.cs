using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IBacklogRepository _backlogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IBacklogRepository backlogRepository,
            IProjectRepository projectRepository,
            IMapper mapper
            )
        {
            _backlogRepository = backlogRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<bool> Delete(Guid id)
        {
            _projectRepository.Delete(id);
            return (await _projectRepository.UnitOfWork.SaveChangesAsync() > 0);
        }

        public async Task<IReadOnlyCollection<ProjectViewModel>> GetProjects()
        {
            var projects = await _projectRepository.GetAll();
            var res = _mapper.Map<List<ProjectViewModel>>(projects);
            return res.AsReadOnly();
        }

        public async Task<ProjectViewModel> Create(Guid userId, CreateProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            UserProject userProject = new()
            {
                UserId = userId,
                ProjectId = project.Id,
                Role = "Leader"
            };

            project.UserProjects = new List<UserProject>()
            {
                userProject
            };

            _projectRepository.Add(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();

            Backlog backlog = new()
            {
                Name = project.Name,
                ProjectId = project.Id
            };

            _backlogRepository.Add(backlog);
            await _backlogRepository.UnitOfWork.SaveChangesAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectViewModel> Update(Guid id, UpdateProjectDto updateProjectDto)
        {
            Project? project = await _projectRepository.GetById(id);
            if (project is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(project));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            project.Name = updateProjectDto.Name!;
            project.Code = updateProjectDto.Code!;
            project.Description = updateProjectDto.Description!;
            project.AvatarUrl = updateProjectDto.AvatarUrl!;
            project.IsFavourite = updateProjectDto.IsFavourite;

            _projectRepository.Update(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<IReadOnlyCollection<ProjectViewModel>> GetProjectsByFilter(Guid leaderId, GetProjectByFilterDto filter)
        {
            var roProjects = await _projectRepository.GetByLeaderId(leaderId, filter);
            return roProjects.Adapt<IReadOnlyCollection<ProjectViewModel>>();
        }

        public async Task<ProjectViewModel> Get(Guid projectId)
        {
            var project = await _projectRepository.GetById(projectId);
            return project.Adapt<ProjectViewModel>();
        }

        public async Task<ProjectViewModel> Get(string projectCode)
        {
            var project = await _projectRepository.GetByCode(projectCode);
            return project.Adapt<ProjectViewModel>();
        }
    }
}
