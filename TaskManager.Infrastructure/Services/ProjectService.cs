﻿using Mapster;
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
        private readonly IUserProjectRepository _userProjectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly IProjectConfigurationRepository _projectConfigurationRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IBacklogRepository backlogRepository,
            IProjectRepository projectRepository,
            IUserProjectRepository userProjectRepository,
            ISprintRepository sprintRepository,
            IIssueTypeRepository issueTypeRepository,
            IProjectConfigurationRepository projectConfigurationRepository,
            IMapper mapper
            )
        {
            _backlogRepository = backlogRepository;
            _projectRepository = projectRepository;
            _userProjectRepository = userProjectRepository;
            _sprintRepository = sprintRepository;
            _issueTypeRepository = issueTypeRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
            _mapper = mapper;
        }

        #region Private Method
        private IReadOnlyCollection<IssueViewModel> ToIssueViewModels(IReadOnlyCollection<Issue> issues)
        {
            var issueViewModels = new List<IssueViewModel>();
            if (issues.Any())
            {
                foreach (var issue in issues)
                {
                    var issueViewModel = ToIssueViewModel(issue);
                    issueViewModels.Add(issueViewModel);
                }
            }
            return issueViewModels.AsReadOnly();
        }

        private IssueViewModel ToIssueViewModel(Issue issue)
        {
            var issueViewModel = _mapper.Map<IssueViewModel>(issue);
            if (issue.IssueDetail is not null)
            {
                var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
                issueViewModel.IssueDetail = issueDetail;
            }
            if (issue.IssueHistories is not null && issue.IssueHistories.Any())
            {
                var issueHistories = _mapper.Map<ICollection<IssueHistoryViewModel>>(issue.IssueHistories);
                issueViewModel.IssueHistories = issueHistories;
            }
            if (issue.Comments is not null && issue.Comments.Any())
            {
                var comments = _mapper.Map<ICollection<CommentViewModel>>(issue.Comments);
                issueViewModel.Comments = comments;
            }
            if (issue.Attachments is not null && issue.Attachments.Any())
            {
                var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(issue.Attachments);
                issueViewModel.Attachments = attachments;
            }
            return issueViewModel;
        }

        private async Task<ProjectViewModel> ToProjectViewModel(Project project)
        {
            var members = await _projectRepository.GetMembers(project.Id);
            var backlog = await _backlogRepository.GetBacklog(project.Id);
            var issueForBacklog = await _backlogRepository.GetIssues(backlog.Id);
            backlog.Issues = ToIssueViewModels(issueForBacklog).ToList();
            var sprints = await _sprintRepository.GetSprintByProjectId(project.Id);
            var issueTypes = await _issueTypeRepository.GetsByProjectId(project.Id);
            if (sprints.Any())
            {
                foreach (var sprint in sprints)
                {
                    var issues = await _sprintRepository.GetIssues(sprint.Id);
                    sprint.Issues = ToIssueViewModels(issues).ToList();
                }
            }
            var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            projectViewModel.Leader = members.Where(m => m.Role == CoreConstants.LeaderRole).SingleOrDefault();
            projectViewModel.Members = members.Where(m => m.Role != CoreConstants.LeaderRole).ToList();
            projectViewModel.Backlog = backlog;
            projectViewModel.Sprints = sprints.ToList();
            projectViewModel.IssueTypes = issueTypes.ToList();
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
        #endregion

        public async Task<Guid> Delete(Guid id)
        {
            _projectRepository.Delete(id);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<IReadOnlyCollection<ProjectViewModel>> GetProjects()
        {
            var projects = await _projectRepository.GetAll();
            return await ToProjectViewModels(projects);
        }

        public async Task<ProjectViewModel> Create(Guid userId, CreateProjectDto projectDto)
        {
            var project = projectDto.Adapt<Project>();
            project.AvatarUrl = "https://bs-uploads.toptal.io/blackfish-uploads/components/skill_page/content/logo_file/logo/195649/JIRA_logo-e5a9c767df8a60eb2d242a356ce3fdca.jpg";
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

            ProjectConfiguration projectConfiguration = new()
            {
                ProjectId = project.Id,
                IssueCode = 1,
                SprintCode = 1,
                Code = project.Code
            };

            _projectConfigurationRepository.Add(projectConfiguration);
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

            return await ToProjectViewModel(project);
        }

        public async Task<ProjectViewModel> Update(Guid userId, Guid projectId, UpdateProjectDto updateProjectDto)
        {
            Project? project = await _projectRepository.GetById(projectId);
            if (project is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(project));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            project = updateProjectDto.Adapt(project);
            if (updateProjectDto.LeaderId is Guid newLeaderId && newLeaderId != userId)
            {
                var oldLeader = _userProjectRepository.Get(projectId, userId);
                if (oldLeader is not null)
                {
                    var existsNewLeader = _userProjectRepository.Get(projectId, newLeaderId);

                    if (existsNewLeader is not null)
                    {
                        existsNewLeader.Role = CoreConstants.LeaderRole;
                        oldLeader.Role = "Member";
                        _userProjectRepository.Update(oldLeader);
                        _userProjectRepository.Update(existsNewLeader);
                    }
                    else
                    {
                        var newLeader = new UserProject
                        {
                            ProjectId = oldLeader.ProjectId,
                            UserId = newLeaderId,
                            Role = CoreConstants.LeaderRole
                        };
                        oldLeader.Role = "Member";
                        _userProjectRepository.Add(newLeader);
                        _userProjectRepository.Update(oldLeader);
                    }
                    await _userProjectRepository.UnitOfWork.SaveChangesAsync();
                }
            }

            _projectRepository.Update(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
            return await ToProjectViewModel(project);
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

            if (addMemberToProjectDto.UserIds is not null && addMemberToProjectDto.UserIds.Any())
            {
                foreach (var userId in addMemberToProjectDto.UserIds)
                {
                    var userProject = new UserProject()
                    {
                        ProjectId = addMemberToProjectDto.ProjectId,
                        UserId = userId,
                        Role = addMemberToProjectDto.Role
                    };
                    project.UserProjects!.Add(userProject);
                }
            }

            _projectRepository.Update(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();

            return await ToProjectViewModel(project);
        }

        public async Task<ProjectViewModel?> Get(string code)
        {
            var project = await _projectRepository.GetByCode(code);
            if (project is null)
            {
                return null;
            }
            return await ToProjectViewModel(project);
        }
    }
}
