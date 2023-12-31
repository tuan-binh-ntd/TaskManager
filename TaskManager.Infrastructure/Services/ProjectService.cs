﻿using Dapper;
using Mapster;
using MapsterMapper;
using TaskManager.Core;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

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
    private readonly INotificationRepository _notificationRepository;
    private readonly IIssueEventRepository _issueEventRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IConnectionFactory _connectionFactory;
    private readonly IIssueDetailRepository _issueDetailRepository;
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
        INotificationRepository notificationRepository,
        IIssueEventRepository issueEventRepository,
        ILabelRepository labelRepository,
        IConnectionFactory connectionFactory,
        IIssueDetailRepository issueDetailRepository,
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
        _notificationRepository = notificationRepository;
        _issueEventRepository = issueEventRepository;
        _labelRepository = labelRepository;
        _connectionFactory = connectionFactory;
        _issueDetailRepository = issueDetailRepository;
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

    private async Task<ProjectViewModel> ToProjectViewModel(Project project, Guid? userId = null)
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
        if (userId is Guid newUserId)
        {
            projectViewModel.UserPermissionGroup = await _permissionGroupRepository.GetPermissionGroupViewModelById(project.Id, newUserId);
            projectViewModel.IsFavourite = await _userProjectRepository.GetIsFavouriteCol(project.Id, newUserId);
        }
        projectViewModel.PermissionGroups = await _permissionGroupRepository.GetByProjectId(project.Id);
        var projectConfiguration = _projectConfigurationRepository.GetByProjectId(project.Id);
        projectViewModel.ProjectConfiguration = projectConfiguration.Adapt<ProjectConfigurationViewModel>();
        var labels = await _labelRepository.GetByProjectId(project.Id); ;
        projectViewModel.Labels = labels.Adapt<IReadOnlyCollection<LabelViewModel>>();
        return projectViewModel;
    }

    private async Task<IReadOnlyCollection<ProjectViewModel>> ToProjectViewModels(IReadOnlyCollection<Project> projects, Guid userId)
    {
        var projectViewModels = new List<ProjectViewModel>();
        if (projects.Any())
        {
            foreach (var item in projects)
            {
                var projectViewModel = await ToProjectViewModel(item, userId);
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
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.HideCode).Select(e => e.Id).FirstOrDefault(),
            IsMain = true,
            AllowAnyStatus = true,
        };

        var anyStatus = new Status()
        {
            Name = CoreConstants.AnyStatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.HideCode).Select(e => e.Id).FirstOrDefault(),
            IsMain = true,
            AllowAnyStatus = true,
        };

        var todoStatus = new Status()
        {
            Name = CoreConstants.TodoStatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.ToDoCode).Select(e => e.Id).FirstOrDefault(),
            Ordering = 1,
            IsMain = true,
            AllowAnyStatus = true,
        };

        var inProgressStatus = new Status()
        {
            Name = CoreConstants.InProgresstatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.InProgressCode).Select(e => e.Id).FirstOrDefault(),
            Ordering = 2,
            IsMain = true,
            AllowAnyStatus = true,
        };

        var doneStatus = new Status()
        {
            Name = CoreConstants.DoneStatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.DoneCode).Select(e => e.Id).FirstOrDefault(),
            Ordering = 3,
            IsMain = true,
            AllowAnyStatus = true,
        };

        var unreleasedStatus = new Status()
        {
            Name = CoreConstants.UnreleasedStatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.VersionCode).Select(e => e.Id).FirstOrDefault(),
            IsMain = true
        };

        var releasedStatus = new Status()
        {
            Name = CoreConstants.ReleasedStatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.VersionCode).Select(e => e.Id).FirstOrDefault(),
            IsMain = true
        };

        var archivedStatus = new Status()
        {
            Name = CoreConstants.ArchivedStatusName,
            ProjectId = project.Id,
            StatusCategoryId = statusCategories.Where(e => e.Code == CoreConstants.VersionCode).Select(e => e.Id).FirstOrDefault(),
            IsMain = true
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

        // Any Transition
        //var anyToTodoTransition = new Transition()
        //{
        //    Name = "Any status moving to \"To Do\"",
        //    FromStatusId = anyStatus.Id,
        //    ToStatusId = todoStatus.Id,
        //    ProjectId = project.Id,
        //};

        //var anyToInProgressTransition = new Transition()
        //{
        //    Name = "Any status moving to \"In Progress\"",
        //    FromStatusId = anyStatus.Id,
        //    ToStatusId = inProgressStatus.Id,
        //    ProjectId = project.Id,
        //};

        //var anyToDoneTransition = new Transition()
        //{
        //    Name = "Any status moving to \"Done\"",
        //    FromStatusId = anyStatus.Id,
        //    ToStatusId = doneStatus.Id,
        //    ProjectId = project.Id,
        //};

        var transitions = new List<Transition>()
        {
            createTransition
        };

        _transitionRepository.AddRange(transitions);
        await _transitionRepository.UnitOfWork.SaveChangesAsync();
    }

    private async Task CreateWorkflowForProject(Project project, IReadOnlyCollection<IssueType> issueTypes)
    {
        var workflow = new Workflow
        {
            Name = $"Workflow of {project.Name}",
            ProjectId = project.Id,
            WorkflowIssueTypes = new List<WorkflowIssueType>()
        };
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

    private async Task CreateBacklogAndProjectConfigurationForProject(Project project, Guid mediumPriorityId)
    {
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
            DefaultPriorityId = mediumPriorityId
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

    private async Task<IReadOnlyCollection<IssueType>> CreateIssueTypesForProject(Project project)
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
            IsMain = true
            },
            new()
            {
                Name = CoreConstants.BugName,
                Description = "Bugs track problems or errors.",
                Icon = CoreConstants.BugIcon,
                Level = 2,
                ProjectId = project.Id,
                IsMain = true
            },
            new()
            {
                Name = CoreConstants.StoryName,
                Description = "Stories track functionality or features expressed as user goals.",
                Icon = CoreConstants.StoryIcon,
                Level = 2,
                ProjectId = project.Id,
                IsMain = true
            },
            new()
            {
                Name = CoreConstants.TaskName,
                Description = "Tasks track small, distinct pieces of work.",
                Icon = CoreConstants.TaskIcon,
                Level = 2,
                ProjectId = project.Id,
                IsMain = true
            },
            new()
            {
                Name = CoreConstants.SubTaskName,
                Description = "Subtasks track small pieces of work that are part of a larger task.",
                Icon = CoreConstants.SubTaskIcon,
                Level = 3,
                ProjectId = project.Id,
                IsMain = true
            }
        };

        _issueTypeRepository.AddRange(issueTypes);
        await _issueTypeRepository.UnitOfWork.SaveChangesAsync();

        return issueTypes.AsReadOnly();
    }

    private async Task<Guid> CreatePrioritiesForProject(Project project)
    {
        var priorities = new List<Priority>()
        {
            new()
            {
                Name = CoreConstants.LowestName,
                Description = "Trivial problem with little or no impact on progress.",
                Color = CoreConstants.LowestColor,
                ProjectId = project.Id,
                Icon = CoreConstants.LowestIcon,
            IsMain = true

            },
            new()
            {
                Name = CoreConstants.LowName,
                Description = "Minor problem or easily worked around.",
                Color = CoreConstants.LowColor,
                ProjectId = project.Id,
                Icon = CoreConstants.LowIcon,
                IsMain = true
            },
            new()
            {
                Name = CoreConstants.MediumName,
                Description = "Has the potential to affect progress.",
                Color = CoreConstants.MediumColor,
                ProjectId = project.Id,
                Icon = CoreConstants.MediumIcon,
                IsMain = true
            },
            new()
            {
                Name = CoreConstants.HighName,
                Description = "Serious problem that could block progress.",
                Color = CoreConstants.HighColor,
                ProjectId = project.Id,
                Icon = CoreConstants.HighIcon,
                IsMain = true
            },
            new()
            {
                Name = CoreConstants.HighestName,
                Description = "This problem will block progress.",
                Color = CoreConstants.HighestColor,
                ProjectId = project.Id,
                Icon = CoreConstants.HighestIcon,
                IsMain = true
            }
        };

        _priorityRepository.AddRange(priorities);
        await _priorityRepository.UnitOfWork.SaveChangesAsync();

        return priorities.Where(p => p.Name == CoreConstants.MediumName).Select(p => p.Id).FirstOrDefault();
    }

    private async Task<Guid> CreateRolesForProject(Project project)
    {
        var plPermissions = new Permissions()
        {
            Timeline = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
            Backlog = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
            Board = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
            Project = new PermissionGroupDto { ViewPermission = true, EditPermission = true },
        };

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

        var projectLeadRole = new PermissionGroup()
        {
            Name = CoreConstants.ProjectLeadName,
            ProjectId = project.Id,
            Permissions = plPermissions.ToJson(),
            IsMain = true,
        };

        var productOwnerRole = new PermissionGroup()
        {
            Name = CoreConstants.ProductOwnerName,
            ProjectId = project.Id,
            Permissions = poPermissions.ToJson(),
            IsMain = true,
        };

        var scrumMasterRole = new PermissionGroup()
        {
            Name = CoreConstants.ScrumMasterName,
            ProjectId = project.Id,
            Permissions = smPermissions.ToJson(),
            IsMain = true,
        };
        var developerRole = new PermissionGroup()
        {
            Name = CoreConstants.DeveloperName,
            ProjectId = project.Id,
            Permissions = devPermissions.ToJson(),
            IsMain = true,
        };

        var permissionGroups = new List<PermissionGroup>()
        {
            projectLeadRole, productOwnerRole, scrumMasterRole, developerRole
        };

        _permissionGroupRepository.AddRange(permissionGroups);
        await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();

        return projectLeadRole.Id;
    }

    private async Task<bool> CreateNotificationScheme(Project project)
    {
        var issueEvents = await _issueEventRepository.Gets();
        var notification = new Notification()
        {
            Name = "Default Notification Scheme",
            NotificationIssueEvents = new List<NotificationIssueEvent>(),
            ProjectId = project.Id,
            Description = "Notification emails for this project can be sent directly from Jira Software, or you can use a custom email with your own domain instead."
        };
        foreach (var item in issueEvents)
        {
            var notificationIssueEvent = new NotificationIssueEvent()
            {
                NotificationId = notification.Id,
                IssueEventId = item.Id,
                AllWatcher = true,
                CurrentAssignee = true,
                ProjectLead = false,
                Reporter = true,
            };
            notification.NotificationIssueEvents!.Add(notificationIssueEvent);
        }
        _notificationRepository.Add(notification);
        return await _notificationRepository.UnitOfWork.SaveChangesAsync() > 0;
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
        await _projectRepository.LoadSprints(project);
        await _projectRepository.LoadVersions(project);

        await _issueRepository.DeleteByProjectId(project.Id);
        if (project.Sprints is not null && project.Sprints.Any())
        {
            foreach (var sprint in project.Sprints)
            {
                await _issueRepository.DeleteBySprintId(sprint.Id);
            }
        }
        if (project.Backlog is not null)
        {
            await _issueRepository.DeleteByBacklogId(project.Backlog.Id);
        }

        _projectRepository.Delete(project);
        await _projectRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<ProjectViewModel> Create(Guid userId, CreateProjectDto projectDto)
    {
        var project = projectDto.Adapt<Project>();
        project.AvatarUrl = "https://bs-uploads.toptal.io/blackfish-uploads/components/skill_page/content/logo_file/logo/195649/JIRA_logo-e5a9c767df8a60eb2d242a356ce3fdca.jpg";

        _projectRepository.Add(project);
        await _projectRepository.UnitOfWork.SaveChangesAsync();

        var mediumPriorityId = await CreatePrioritiesForProject(project);

        await CreateBacklogAndProjectConfigurationForProject(project, mediumPriorityId);

        await CreateStatusForProject(project);

        var issueTypes = await CreateIssueTypesForProject(project);

        await CreateWorkflowForProject(project, issueTypes);

        var productOwnerId = await CreateRolesForProject(project);

        UserProject userProject = new()
        {
            UserId = userId,
            ProjectId = project.Id,
            Role = "Leader",
            PermissionGroupId = productOwnerId
        };

        _userProjectRepository.Add(userProject);
        await _projectRepository.UnitOfWork.SaveChangesAsync();

        await CreateNotificationScheme(project);

        return await ToProjectViewModel(project, userId);
    }

    public async Task<ProjectViewModel> Update(Guid userId, Guid projectId, UpdateProjectDto updateProjectDto)
    {
        Project? project = await _projectRepository.GetById(projectId) ?? throw new ProjectNullException();

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

        if(updateProjectDto.IsFavourite is bool isFavourite)
        {
            await _userProjectRepository.UpdateIsFavouriteCol(projectId, userId, isFavourite);
        }

        var projectConfiguration = _projectConfigurationRepository.GetByProjectId(projectId);
        projectConfiguration.DefaultAssigneeId = updateProjectDto.DefaultAssigneeId;
        projectConfiguration.DefaultPriorityId = updateProjectDto.DefaultPriorityId;
        projectConfiguration.Code = string.IsNullOrWhiteSpace(updateProjectDto.Code) ? projectConfiguration.Code : updateProjectDto.Code;

        _projectConfigurationRepository.Update(projectConfiguration);
        _projectRepository.Update(project);
        await _projectRepository.UnitOfWork.SaveChangesAsync();
        return await ToProjectViewModel(project, userId);
    }

    public async Task<object> GetProjectsByFilter(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput)
    {
        if (paginationInput.IsPaging())
        {
            var pagedProjects = await _projectRepository.GetByUserId(userId, filter, paginationInput);
            var res = new PaginationResult<ProjectViewModel>()
            {
                TotalCount = pagedProjects.TotalCount,
                TotalPage = pagedProjects.TotalPage,
                Content = await ToProjectViewModels(pagedProjects.Content!, userId)
            };
            return res;
        }
        else
        {
            var res = await _projectRepository.GetByUserId(userId, filter);
            return await ToProjectViewModels(res, userId);
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
                    Role = addMemberToProjectDto.Role,
                    PermissionGroupId = addMemberToProjectDto.PermissionGroupId
                };
                project.UserProjects!.Add(userProject);
            }
        }

        _projectRepository.Update(project);
        await _projectRepository.UnitOfWork.SaveChangesAsync();

        return await ToProjectViewModel(project);
    }

    public async Task<ProjectViewModel?> Get(string code, Guid id)
    {
        var project = await _projectRepository.GetByCode(code);
        if (project is null)
        {
            return null;
        }
        return await ToProjectViewModel(project, id);
    }

    public async Task<object> GetMembersOfProject(Guid projectId, PaginationInput paginationInput)
    {
        var members = await _userProjectRepository.GetMemberProjects(projectId, paginationInput);
        return members;
    }

    public async Task<MemberProjectViewModel> UpdateMembder(Guid id, UpdateMemberProjectDto updateMemberProjectDto)
    {
        var userProject = await _userProjectRepository.GetMember(id) ?? throw new MemberProjectNullException();
        userProject.PermissionGroupId = updateMemberProjectDto.PermissionGroupId;
        _userProjectRepository.Update(userProject);
        await _projectRepository.UnitOfWork.SaveChangesAsync();
        return await _userProjectRepository.GetMemberProject(id) ?? throw new MemberProjectViewModelNullException();
    }

    public async Task<Guid> DeleteMember(Guid projectId, Guid id)
    {
        var defaultAssigneeId = await _projectConfigurationRepository.GetDefaultAssigneeIdByProjectId(projectId);
        var leaderId = await _userProjectRepository.GetLeaderIdByProjectId(projectId);


        var userProject = await _userProjectRepository.GetMember(id) ?? throw new MemberProjectNullException();

        if (userProject.UserId == defaultAssigneeId)
        {
            await _projectConfigurationRepository.UpdateDefaultAssignee(projectId, leaderId);
            defaultAssigneeId = leaderId;
        }

        await _issueDetailRepository.UpdateOneColumnForIssueDetail(userProject.UserId, defaultAssigneeId, NameColumn.AssigneeId);

        await _issueDetailRepository.UpdateOneColumnForIssueDetail(userProject.UserId, leaderId, NameColumn.ReporterId);

        _userProjectRepository.Delete(userProject);
        await _userProjectRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<IReadOnlyCollection<SprintFilterViewModel>> GetSprintFiltersViewModel(Guid projectId)
    {
        var sprintFilterViewModels = await _projectRepository.GetSprintFiltersByProjectId(projectId);
        return sprintFilterViewModels;
    }

    public async Task<IReadOnlyCollection<TypeFilterViewModel>> GetTypeFiltersViewModel(Guid projectId)
    {
        var typeFilterViewModels = await _projectRepository.GetIssueTypeFiltersByProjectId(projectId);
        return typeFilterViewModels;
    }

    public async Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersViewModel(Guid projectId)
    {
        var epicFilterViewModels = await _projectRepository.GetEpicFiltersByProjectId(projectId);
        return epicFilterViewModels;
    }

    public async Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersViewModel(Guid projectId)
    {
        var labelFilterViewModels = await _projectRepository.GetLabelFiltersByProjectId(projectId);
        return labelFilterViewModels;
    }

    public async Task<GetIssueForProjectViewModel> GetIssueForProjectViewModelAsync(Guid projectId, GetIssueForProjectFilterInputModel getIssueForProjectFilterInputModel)
    {
        var backlog = await _backlogRepository.GetBacklog(projectId);

        var getIssueForProjectFilterDto = new GetIssueForProjectFilterDto
        {
            VerionIds = getIssueForProjectFilterInputModel.VerionIds,
            EpicIds = getIssueForProjectFilterInputModel.EpicIds,
            LabelIds = getIssueForProjectFilterInputModel.LabelIds,
            IssueTypeIds = getIssueForProjectFilterInputModel.IssueTypeIds,
            SearchKey = getIssueForProjectFilterInputModel.SearchKey,
            BacklogId = backlog.Id
        };

        string query = getIssueForProjectFilterDto.FullQuery();
        var issueIds = await _connectionFactory.QueryAsync<Guid>(query);
        var issues = await _issueRepository.GetByIds(issueIds);

        var issueOfbacklog = issues.Where(i => i.BacklogId == backlog.Id).ToList();
        var issueViewModels = await ToIssueViewModels(issueOfbacklog);
        backlog.Issues = issueViewModels;

        var sprints = await _sprintRepository.GetSprintByProjectId(projectId);
        if (sprints.Any())
        {
            foreach (var sprint in sprints)
            {
                var issuesOfSprint = issues.Where(i => i.SprintId == sprint.Id).ToList();
                issueViewModels = await ToIssueViewModels(issuesOfSprint);
                sprint.Issues = issueViewModels.ToList();
            }
        }

        return new GetIssueForProjectViewModel
        {
            Sprints = sprints,
            Backlog = backlog,
        };
    }

    public async Task<IReadOnlyCollection<VersionFilterViewModel>> GetVerionFiltersViewModelForBacklog(Guid projectId)
    {
        string query = @"
            SELECT
              v.Id,
              v.Name
            FROM Sprints s 
            JOIN Issues i ON s.Id = i.SprintId
            JOIN VersionIssues vi ON i.Id = vi.IssueId
            JOIN Versions v ON vi.VersionId = v.Id
            WHERE s.ProjectId = @ProjectId AND s.IsComplete <> 1
            GROUP BY v.Id, v.Name
            UNION
            SELECT
              v.Id,
              v.Name
            FROM Backlogs b
            JOIN Issues i ON b.Id = i.BacklogId
            JOIN VersionIssues vi ON i.Id = vi.IssueId
            JOIN Versions v ON vi.VersionId = v.Id
            WHERE b.ProjectId = @ProjectId
            GROUP BY v.Id, v.Name
        ";

        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, System.Data.DbType.Guid);
        var versionFilters = await _connectionFactory.QueryAsync<VersionFilterViewModel>(query, param);

        return versionFilters.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<TypeFilterViewModel>> GetTypeFiltersViewModelForBacklog(Guid projectId)
    {
        string query = @"
            SELECT
              it.Id,
              it.Name
            FROM Sprints s 
            JOIN Issues i ON s.Id = i.SprintId
            JOIN IssueTypes it ON i.IssueTypeId = it.Id
            WHERE s.ProjectId = @ProjectId AND s.IsComplete <> 1
            GROUP BY it.Id, it.Name
            UNION
            SELECT
              it.Id,
              it.Name
            FROM Backlogs b
            JOIN Issues i ON b.Id = i.BacklogId
            JOIN IssueTypes it ON i.IssueTypeId = it.Id
            WHERE b.ProjectId = @ProjectId
            GROUP BY it.Id, it.Name
        ";

        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, System.Data.DbType.Guid);
        var typeFilters = await _connectionFactory.QueryAsync<TypeFilterViewModel>(query, param);

        return typeFilters.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersViewModelForBacklog(Guid projectId)
    {
        string query = @"
            SELECT
              e.Id,
              e.Name
            FROM Sprints s 
            JOIN Issues i ON s.Id = i.SprintId
            JOIN Issues e ON i.ParentId = e.Id
            WHERE s.ProjectId = @ProjectId AND s.IsComplete <> 1
            GROUP BY e.Id, e.Name
            UNION
            SELECT
              e.Id,
              e.Name
            FROM Backlogs b
            JOIN Issues i ON b.Id = i.BacklogId
            JOIN Issues e ON i.ParentId = e.Id
            WHERE b.ProjectId = @ProjectId
            GROUP BY e.Id, e.Name
        ";

        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, System.Data.DbType.Guid);
        var epicFilters = await _connectionFactory.QueryAsync<EpicFilterViewModel>(query, param);

        return epicFilters.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersViewModelForBacklog(Guid projectId)
    {
        string query = @"
            SELECT
              l.Id,
              l.Name
            FROM Sprints s 
            JOIN Issues i ON s.Id = i.SprintId
            JOIN LabelIssues li ON i.Id = li.IssueId
            JOIN Labels l ON li.LabelId = l.Id
            WHERE s.ProjectId = @ProjectId AND s.IsComplete <> 1
            GROUP BY l.Id, l.Name
            UNION
            SELECT
              l.Id,
              l.Name
            FROM Backlogs b
            JOIN Issues i ON b.Id = i.BacklogId
            JOIN LabelIssues li ON i.Id = li.IssueId
            JOIN Labels l ON li.LabelId = l.Id
            WHERE b.ProjectId = @ProjectId
            GROUP BY l.Id, l.Name
        ";

        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, System.Data.DbType.Guid);
        var labelFilters = await _connectionFactory.QueryAsync<LabelFilterViewModel>(query, param);

        return labelFilters.ToList().AsReadOnly();
    }
}
