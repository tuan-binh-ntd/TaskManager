using Mapster;
using MapsterMapper;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
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
            return await ToProjectViewModels(projects);
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

        public async Task<object> GetProjectsByFilter(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput)
        {
            if (paginationInput.pagenum is not default(int) && paginationInput.pagesize is not default(int))
            {
                var pagedProjects = await _projectRepository.GetByUserId(userId, filter, paginationInput);
                var res = new PaginationResult<ProjectViewModel>()
                {
                    TotalCount = pagedProjects.TotalCount,
                    TotalPage = pagedProjects.TotalPage,
                    Content = await ToProjectViewModels(pagedProjects.Content!)
                };
                return res;
            }
            else
            {
                var res = await _projectRepository.GetByUserId(userId, filter);
                return await ToProjectViewModels(res);
            }
        }

        public async Task<ProjectViewModel> Get(Guid projectId)
        {
            var project = await _projectRepository.GetById(projectId);
            return project.Adapt<ProjectViewModel>();
        }

        public async Task<ProjectViewModel> AddMember(AddMemberToProjectDto addMemberToProjectDto)
        {
            var project = await _projectRepository.GetById(addMemberToProjectDto.ProjectId);
            if (project is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(project));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            var userProject = new UserProject()
            {
                ProjectId = addMemberToProjectDto.ProjectId,
                UserId = addMemberToProjectDto.UserId,
                Role = addMemberToProjectDto.Role
            };

            project.UserProjects!.Add(userProject);
                
            _projectRepository.Update(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();

            return project.Adapt<ProjectViewModel>();
        }

        #region Private Method
        private async Task<ProjectViewModel> ToProjectViewModel(Project project)
        {
            var members = await _projectRepository.GetMembers(project.Id);
            var projectViewModel = project.Adapt<ProjectViewModel>();
            projectViewModel.Leader = members.Where(m => m.Role == CoreConstants.LeaderRole).SingleOrDefault();
            projectViewModel.Members = members.Where(m => m.Role != CoreConstants.LeaderRole).ToList();
            return projectViewModel;
        }

        private async Task<IReadOnlyCollection<ProjectViewModel>> ToProjectViewModels(IReadOnlyCollection<Project> projects)
        {
            var projectViewModels = new List<ProjectViewModel>();
            if (projects.Any())
            {
                foreach (var item in projects)
                {
                    var projectViewModel = await ToProjectViewModel(item);
                    projectViewModels.Add(projectViewModel);
                }
                return projectViewModels.AsReadOnly();
            }
            return projectViewModels;
        }

        public async Task<ProjectViewModel?> Get(string code)
        {
            var project = await _projectRepository.GetByCode(code);
            if (project is null)
            {
                return null;
            }
            return project.Adapt<ProjectViewModel>();
        }
        #endregion
    }
}
