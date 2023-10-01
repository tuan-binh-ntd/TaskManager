using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
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

        public async Task<ProjectViewModel> Create(Guid userId, ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            project.LeaderId = userId;
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

        public async Task<ProjectViewModel> Update(Guid id, ProjectDto projectDto)
        {
            Project? project = await _projectRepository.GetById(id);
            if (project is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(project));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            project = _mapper.Map<Project>(projectDto);
            //_projectRepository.Update(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<IReadOnlyCollection<ProjectViewModel>> GetProjectsByFilter(Guid leaderId, GetProjectByFilterDto filter)
        {
            var roProjects = await _projectRepository.GetByLeaderId(leaderId);

            var projects = roProjects
                .WhereIf(string.IsNullOrWhiteSpace(filter.Name), p => p.Name.Contains(filter.Name))
                .WhereIf(string.IsNullOrWhiteSpace(filter.Code), p => p.Code.Contains(filter.Code))
                .ToList();

            return _mapper.Adapt<IReadOnlyCollection<ProjectViewModel>>();
        }
    }
}
