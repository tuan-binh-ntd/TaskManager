﻿using Mapster;
using MapsterMapper;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
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
        private readonly IStatusCategoryRepository _statusCategoryRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ITransitionRepository _transitionRepository;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IPriorityRepository _priorityRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;
        private readonly IStatusCategoryRepository _stateCategoryRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IBacklogRepository backlogRepository,
            IProjectRepository projectRepository,
            IUserProjectRepository userProjectRepository,
            ISprintRepository sprintRepository,
            IIssueTypeRepository issueTypeRepository,
            IProjectConfigurationRepository projectConfigurationRepository,
            IStatusCategoryRepository statusCategoryRepository,
            IStatusRepository statusRepository,
            ITransitionRepository transitionRepository,
            IWorkflowRepository workflowRepository,
            IIssueRepository issueRepository,
            IPriorityRepository priorityRepository,
            IPermissionGroupRepository permissionGroupRepository,
            IStatusCategoryRepository stateCategoryRepository,
            IMapper mapper
            )
        {
            _backlogRepository = backlogRepository;
            _projectRepository = projectRepository;
            _userProjectRepository = userProjectRepository;
            _sprintRepository = sprintRepository;
            _issueTypeRepository = issueTypeRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
            _statusCategoryRepository = statusCategoryRepository;
            _statusRepository = statusRepository;
            _transitionRepository = transitionRepository;
            _workflowRepository = workflowRepository;
            _issueRepository = issueRepository;
            _priorityRepository = priorityRepository;
            _permissionGroupRepository = permissionGroupRepository;
            _stateCategoryRepository = stateCategoryRepository;
            _mapper = mapper;
        }

        #region Private Method
        private async Task<IReadOnlyCollection<IssueViewModel>> ToIssueViewModels(IReadOnlyCollection<Issue> issues)
        {
            var issueViewModels = new List<IssueViewModel>();
            if (issues.Any())
            {
                foreach (var issue in issues)
                {
                    var issueViewModel = await ToIssueViewModel(issue);
                    issueViewModels.Add(issueViewModel);
                }
            }
            return issueViewModels.AsReadOnly();
        }

        private async Task<IssueViewModel> ToIssueViewModel(Issue issue)
        {
            await _issueRepository.LoadAttachments(issue);
            await _issueRepository.LoadIssueDetail(issue);
            await _issueRepository.LoadIssueType(issue);
            await _issueRepository.LoadStatus(issue);
            var issueViewModel = _mapper.Map<IssueViewModel>(issue);

            if (issue.IssueDetail is not null)
            {
                var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
                issueViewModel.IssueDetail = issueDetail;
            }
            if (issue.Attachments is not null && issue.Attachments.Any())
            {
                var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(issue.Attachments);
                issueViewModel.Attachments = attachments;
            }
            if (issue.IssueType is not null)
            {
                var issueType = _mapper.Map<IssueTypeViewModel>(issue.IssueType);
                issueViewModel.IssueType = issueType;
            }
            if (issue.Status is not null)
            {
                var status = _mapper.Map<StatusViewModel>(issue.Status);
                issueViewModel.Status = status;
            }
            if (issue.ParentId is Guid parentId)
            {
                issueViewModel.ParentName = await _issueRepository.GetParentName(parentId);
            }
            return issueViewModel;
        }

        private async Task<ProjectViewModel> ToProjectViewModel(Project project)
        {
            var members = await _projectRepository.GetMembers(project.Id);
            var backlog = await _backlogRepository.GetBacklog(project.Id);
            var issueForBacklog = await _backlogRepository.GetIssues(backlog.Id);
            var issueViewModels = await ToIssueViewModels(issueForBacklog);
            backlog.Issues = issueViewModels.ToList();
            var sprints = await _sprintRepository.GetSprintByProjectId(project.Id);
            var issueTypes = await _issueTypeRepository.GetsByProjectId(project.Id);
            var statuses = await _statusRepository.GetByProjectId(project.Id);
            var epics = await _issueRepository.GetEpicByProjectId(project.Id);
            var priorities = await _priorityRepository.GetByProjectId(project.Id);
            var permissionGroups = await _permissionGroupRepository.GetByProjectId(project.Id);
            var statusCategories = _stateCategoryRepository.Gets();
            if (sprints.Any())
            {
                foreach (var sprint in sprints)
                {
                    var issues = await _sprintRepository.GetIssues(sprint.Id, project.Id);
                    issueViewModels = await ToIssueViewModels(issues);
                    sprint.Issues = issueViewModels.ToList();
                }
            }
            var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            projectViewModel.Leader = members.Where(m => m.Role == CoreConstants.LeaderRole).SingleOrDefault();
            projectViewModel.Members = members.Where(m => m.Role != CoreConstants.LeaderRole).ToList();
            projectViewModel.Backlog = backlog;
            projectViewModel.Sprints = sprints;
            projectViewModel.IssueTypes = issueTypes;
            projectViewModel.Statuses = statuses.Adapt<IReadOnlyCollection<StatusViewModel>>();
            var epicViewModels = await ToEpicViewModels(epics);
            projectViewModel.Epics = epicViewModels.ToList();
            projectViewModel.Priorities = _mapper.Map<IReadOnlyCollection<PriorityViewModel>>(priorities);
            projectViewModel.StatusCategories = _mapper.Map<IReadOnlyCollection<StatusCategoryViewModel>>(statusCategories);
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

        private async Task CreateStatusForProject(Project project)
        {
            // Define default status for project
            var statusCategories = _statusCategoryRepository.Gets();

            var startStatus = new Status()
            {
                Name = CoreConstants.StartStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.HideCode).Select(e => e.Id).FirstOrDefault()
            };

            var anyStatus = new Status()
            {
                Name = CoreConstants.AnyStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.HideCode).Select(e => e.Id).FirstOrDefault()
            };

            var todoStatus = new Status()
            {
                Name = CoreConstants.TodoStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.ToDoCode).Select(e => e.Id).FirstOrDefault(),
                Ordering = 1
            };

            var inProgressStatus = new Status()
            {
                Name = CoreConstants.InProgresstatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.InProgressCode).Select(e => e.Id).FirstOrDefault(),
                Ordering = 2

            };

            var doneStatus = new Status()
            {
                Name = CoreConstants.DoneStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.DoneCode).Select(e => e.Id).FirstOrDefault(),
                Ordering = 3
            };

            var unreleasedStatus = new Status()
            {
                Name = CoreConstants.UnreleasedStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.VersionCode).Select(e => e.Id).FirstOrDefault()
            };

            var releasedStatus = new Status()
            {
                Name = CoreConstants.ReleasedStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.VersionCode).Select(e => e.Id).FirstOrDefault()
            };

            var archivedStatus = new Status()
            {
                Name = CoreConstants.ArchivedStatusName,
                ProjectId = project.Id,
                StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.VersionCode).Select(e => e.Id).FirstOrDefault()
            };

            var statuses = new List<Status>()
            {
                startStatus, todoStatus, inProgressStatus, doneStatus, anyStatus, unreleasedStatus, releasedStatus, archivedStatus,
            };

            _statusRepository.AddRange(statuses);
            await _statusRepository.UnitOfWork.SaveChangesAsync();

            // Define default workflow for project

            // Create transition
            var createTransition = new Transition()
            {
                Name = CoreConstants.CreateTransitionName,
                FromStatusId = startStatus.Id,
                ToStatusId = todoStatus.Id,
                ProjectId = project.Id,
            };

            // Create transition
            var workingTransition = new Transition()
            {
                Name = CoreConstants.WorkingTransitionName,
                FromStatusId = todoStatus.Id,
                ToStatusId = inProgressStatus.Id,
                ProjectId = project.Id,
            };

            // Create transition
            var doneTransition = new Transition()
            {
                Name = CoreConstants.FinishedTransitionName,
                FromStatusId = inProgressStatus.Id,
                ToStatusId = doneStatus.Id,
                ProjectId = project.Id,
            };

            // Any Transition
            var anyToTodoTransition = new Transition()
            {
                Name = "Any status moving to \"To Do\"",
                FromStatusId = anyStatus.Id,
                ToStatusId = todoStatus.Id,
                ProjectId = project.Id,
            };

            var anyToInProgressTransition = new Transition()
            {
                Name = "Any status moving to \"In Progress\"",
                FromStatusId = anyStatus.Id,
                ToStatusId = inProgressStatus.Id,
                ProjectId = project.Id,
            };

            var anyToDoneTransition = new Transition()
            {
                Name = "Any status moving to \"Done\"",
                FromStatusId = anyStatus.Id,
                ToStatusId = doneStatus.Id,
                ProjectId = project.Id,
            };

            var transitions = new List<Transition>()
            {
                createTransition, workingTransition, doneTransition, anyToTodoTransition, anyToInProgressTransition, anyToDoneTransition
            };

            _transitionRepository.AddRange(transitions);
            await _transitionRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task CreateWorkflowForProject(Project project)
        {
            var workflow = new Workflow()
            {
                Name = $"Workflow of {project.Name}",
                ProjectId = project.Id,
            };

            var issueTypes = await _issueTypeRepository.GetsByProjectId(project.Id);
            workflow.WorkflowIssueTypes = new List<WorkflowIssueType>();
            foreach (var item in issueTypes)
            {
                var WorkflowIssueType = new WorkflowIssueType()
                {
                    IssueTypeId = item.Id,
                    WorkflowId = workflow.Id,
                };
                workflow.WorkflowIssueTypes.Add(WorkflowIssueType);
            }

            _workflowRepository.Add(workflow);
            await _workflowRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task CreateBacklogAndProjectConfigurationForProject(Project project)
        {
            var mediumPriority = await _priorityRepository.GetMediumByProjectId(project.Id);
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
                IssueCode = 0,
                SprintCode = 0,
                Code = project.Code,
                DefaultPriorityId = mediumPriority.Id,
            };

            _projectConfigurationRepository.Add(projectConfiguration);
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task<IReadOnlyCollection<EpicViewModel>> ToEpicViewModels(IReadOnlyCollection<Issue> epics)
        {
            var epicViewModels = new List<EpicViewModel>();
            if (epics.Any())
            {
                foreach (var issue in epics)
                {
                    var epicViewModel = await ToEpicViewModel(issue);
                    epicViewModels.Add(epicViewModel);
                }
            }
            return epicViewModels.AsReadOnly();
        }

        private async Task<EpicViewModel> ToEpicViewModel(Issue epic)
        {
            await _issueRepository.LoadEntitiesRelationship(epic);
            var epicViewModel = _mapper.Map<EpicViewModel>(epic);

            if (epic.IssueDetail is not null)
            {
                var issueDetail = _mapper.Map<IssueDetailViewModel>(epic.IssueDetail);
                epicViewModel.IssueDetail = issueDetail;
            }
            if (epic.IssueHistories is not null && epic.IssueHistories.Any())
            {
                var issueHistories = _mapper.Map<ICollection<IssueHistoryViewModel>>(epic.IssueHistories);
                epicViewModel.IssueHistories = issueHistories;
            }
            if (epic.Comments is not null && epic.Comments.Any())
            {
                var comments = _mapper.Map<ICollection<CommentViewModel>>(epic.Comments);
                epicViewModel.Comments = comments;
            }
            if (epic.Attachments is not null && epic.Attachments.Any())
            {
                var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(epic.Attachments);
                epicViewModel.Attachments = attachments;
            }
            if (epic.IssueType is not null)
            {
                var issueType = _mapper.Map<IssueTypeViewModel>(epic.IssueType);
                epicViewModel.IssueType = issueType;
            }
            if (epic.Status is not null)
            {
                var status = _mapper.Map<StatusViewModel>(epic.Status);
                epicViewModel.Status = status;
            }
            var childIssues = await _issueRepository.GetChildIssueOfEpic(epic.Id);
            if (childIssues.Any())
            {
                epicViewModel.ChildIssues = await ToIssueViewModels(childIssues);
            }
            return epicViewModel;
        }

        private async Task CreateIssueTypesForProject(Project project)
        {
            var issueTypes = new List<IssueType>()
            {
                new()
                {
                    Name = CoreConstants.EpicName,
                    Description = "Epics track collections of related bugs, stories, and tasks.",
                    Icon = CoreConstants.EpicIcon,
                    Level = 1,
                    ProjectId = project.Id,
                },
                new()
                {
                    Name = CoreConstants.BugName,
                    Description = "Bugs track problems or errors.",
                    Icon = CoreConstants.BugIcon,
                    Level = 2,
                    ProjectId = project.Id,
                },
                new()
                {
                    Name = CoreConstants.StoryName,
                    Description = "Stories track functionality or features expressed as user goals.",
                    Icon = CoreConstants.StoryIcon,
                    Level = 2,
                    ProjectId = project.Id,
                },
                new()
                {
                    Name = CoreConstants.TaskName,
                    Description = "Tasks track small, distinct pieces of work.",
                    Icon = CoreConstants.TaskIcon,
                    Level = 2,
                    ProjectId = project.Id,
                },
                new()
                {
                    Name = CoreConstants.SubTaskName,
                    Description = "Subtasks track small pieces of work that are part of a larger task.",
                    Icon = CoreConstants.SubTaskIcon,
                    Level = 3,
                    ProjectId = project.Id,
                }
            };

            _issueTypeRepository.AddRange(issueTypes);
            await _issueTypeRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task CreatePrioritiesForProject(Project project)
        {
            var priorities = new List<Priority>()
            {
                new()
                {
                    Name = CoreConstants.LowestName,
                    Description = "Trivial problem with little or no impact on progress.",
                    Color = CoreConstants.LowestColor,
                    ProjectId = project.Id,
                    Icon = CoreConstants.LowestIcon
                },
                new()
                {
                    Name = CoreConstants.LowName,
                    Description = "Minor problem or easily worked around.",
                    Color = CoreConstants.LowColor,
                    ProjectId = project.Id,
                    Icon = CoreConstants.LowIcon
                },
                new()
                {
                    Name = CoreConstants.MediumName,
                    Description = "Has the potential to affect progress.",
                    Color = CoreConstants.MediumColor,
                    ProjectId = project.Id,
                    Icon = CoreConstants.MediumIcon
                },
                new()
                {
                    Name = CoreConstants.HighName,
                    Description = "Serious problem that could block progress.",
                    Color = CoreConstants.HighColor,
                    ProjectId = project.Id,
                    Icon = CoreConstants.HighIcon
                },
                new()
                {
                    Name = CoreConstants.HighestName,
                    Description = "This problem will block progress.",
                    Color = CoreConstants.HighestColor,
                    ProjectId = project.Id,
                    Icon = CoreConstants.HighestIcon
                }
            };

            _priorityRepository.AddRange(priorities);
            await _priorityRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task CreateRolesForProject(Project project)
        {
            //var permissions = await _permissionRepository.GetAll();
            //var productOwnerPermissionRoles = new List<PermissionRole>();
            //var scrumMasterPermissionRoles = new List<PermissionRole>();
            //var developerPermissionRoles = new List<PermissionRole>();

            var poPermissions = new Permissions()
            {
                Timeline = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Backlog = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Board = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Project = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
            };

            var smPermissions = new Permissions()
            {
                Timeline = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Backlog = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Board = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Project = new PermissionGroupDto { ViewPermission = true, EditPermission = false },
            };

            var devPermissions = new Permissions()
            {
                Timeline = new PermissionGroupDto { ViewPermission = false, EditPermission = false },
                Backlog = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
                Board = new PermissionGroupDto { ViewPermission = false, EditPermission = false },
                Project = new PermissionGroupDto { ViewPermission = false, EditPermission = false },
            };

            var productOwnerRole = new PermissionGroup()
            {
                Name = CoreConstants.ProductOwnerName,
                ProjectId = project.Id,
                Permissions = poPermissions.ToJson()
            };

            var scrumMasterRole = new PermissionGroup()
            {
                Name = CoreConstants.ScrumMasterName,
                ProjectId = project.Id,
                Permissions = smPermissions.ToJson()
            };
            var developerRole = new PermissionGroup()
            {
                Name = CoreConstants.DeveloperName,
                ProjectId = project.Id,
                Permissions = devPermissions.ToJson()
            };

            var permissionGroups = new List<PermissionGroup>()
            {
                productOwnerRole, scrumMasterRole, developerRole
            };

            //foreach (var permission in permissions)
            //{
            //    var permissionRole = new PermissionRole()
            //    {
            //        PermissionId = permission.Id,
            //        PermissionGroupId = productOwnerRole.Id,

            //    };
            //    productOwnerPermissionRoles.Add(permissionRole);

            //    if (permission.Name != CoreConstants.ProjectPermissionName)
            //    {
            //        permissionRole = new PermissionRole()
            //        {
            //            PermissionId = permission.Id,
            //            PermissionGroupId = scrumMasterRole.Id,
            //            ViewPermission = true,
            //            EditPermission = true,
            //        };

            //        scrumMasterPermissionRoles.Add(permissionRole);
            //    }
            //    else
            //    {
            //        permissionRole = new PermissionRole()
            //        {
            //            PermissionId = permission.Id,
            //            PermissionGroupId = scrumMasterRole.Id,
            //            ViewPermission = true,
            //            EditPermission = false,
            //        };

            //        scrumMasterPermissionRoles.Add(permissionRole);
            //    }
            //    if (permission.Name == CoreConstants.BacklogPermissionName)
            //    {

            //        permissionRole = new PermissionRole()
            //        {
            //            PermissionId = permission.Id,
            //            PermissionGroupId = developerRole.Id,
            //            ViewPermission = true,
            //            EditPermission = true,
            //        };

            //        developerPermissionRoles.Add(permissionRole);
            //    }
            //    else
            //    {
            //        permissionRole = new PermissionRole()
            //        {
            //            PermissionId = permission.Id,
            //            PermissionGroupId = developerRole.Id,
            //            ViewPermission = false,
            //            EditPermission = false,
            //        };

            //        developerPermissionRoles.Add(permissionRole);
            //    }
            //}

            //productOwnerRole.PermissionRoles = productOwnerPermissionRoles;
            //scrumMasterRole.PermissionRoles = scrumMasterPermissionRoles;
            //developerRole.PermissionRoles = developerPermissionRoles;

            _permissionGroupRepository.AddRange(permissionGroups);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
        }
        #endregion

        public async Task<Guid> Delete(Guid id)
        {
            var project = await _projectRepository.GetById(id) ?? throw new ProjectNullException();
            await _projectRepository.LoadIssueTypes(project);
            await _projectRepository.LoadStatuses(project);
            await _projectRepository.LoadBacklog(project);
            await _projectRepository.LoadUserProjects(project);
            await _projectRepository.LoadProjectConfiguration(project);
            await _projectRepository.LoadTransition(project);
            await _projectRepository.LoadWorkflow(project);
            await _projectRepository.LoadPriorities(project);
            await _projectRepository.LoadPermissionGroup(project);
            _projectRepository.Delete(project);
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

            await CreatePrioritiesForProject(project);

            await CreateBacklogAndProjectConfigurationForProject(project);

            await CreateStatusForProject(project);

            await CreateWorkflowForProject(project);

            await CreateIssueTypesForProject(project);

            await CreateRolesForProject(project);

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

            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(projectId);
            projectConfiguration.DefaultAssigneeId = updateProjectDto.DefaultAssigneeId;
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

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
