using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class EpicService : IEpicService
{
    private readonly IIssueRepository _issueRepository;
    private readonly IProjectConfigurationRepository _projectConfigurationRepository;
    private readonly ITransitionRepository _transitionRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIssueTypeRepository _issueTypeRepository;
    private readonly IIssueDetailRepository _issueDetailRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository;
    private readonly IBacklogRepository _backlogRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IPriorityRepository _priorityRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IEmailSender _emailSender;
    private readonly IVersionRepository _versionRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly IMapper _mapper;

    public EpicService(
        IIssueRepository issueRepository,
        IProjectConfigurationRepository projectConfigurationRepository,
        ITransitionRepository transitionRepository,
        UserManager<AppUser> userManager,
        IIssueTypeRepository issueTypeRepository,
        IIssueDetailRepository issueDetailRepository,
        IIssueHistoryRepository issueHistoryRepository,
        IBacklogRepository backlogRepository,
        ISprintRepository sprintRepository,
        IPriorityRepository priorityRepository,
        IStatusRepository statusRepository,
        IEmailSender emailSender,
        IVersionRepository versionRepository,
        ILabelRepository labelRepository,
        INotificationRepository notificationRepository,
        IProjectRepository projectRepository,
        IUserNotificationRepository userNotificationRepository,
        IMapper mapper
        )
    {
        _issueRepository = issueRepository;
        _projectConfigurationRepository = projectConfigurationRepository;
        _transitionRepository = transitionRepository;
        _userManager = userManager;
        _issueTypeRepository = issueTypeRepository;
        _issueDetailRepository = issueDetailRepository;
        _issueHistoryRepository = issueHistoryRepository;
        _backlogRepository = backlogRepository;
        _sprintRepository = sprintRepository;
        _priorityRepository = priorityRepository;
        _statusRepository = statusRepository;
        _emailSender = emailSender;
        _versionRepository = versionRepository;
        _labelRepository = labelRepository;
        _notificationRepository = notificationRepository;
        _projectRepository = projectRepository;
        _userNotificationRepository = userNotificationRepository;
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
        await _issueRepository.LoadEntitiesRelationship(issue);
        var issueViewModel = _mapper.Map<IssueViewModel>(issue);
        var labelsOfIssue = await _labelRepository.GetByIssueId(issue.Id);
        var versionsOfIssue = await _versionRepository.GetStatusViewModelsByIssueId(issue.Id);

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
        var childIssues = await _issueRepository.GetChildIssueOfIssue(issue.Id);
        if (childIssues.Any())
        {
            issueViewModel.ChildIssues = await ToChildIssueViewModels(childIssues);
        }
        issueViewModel.IssueDetail!.Labels = labelsOfIssue;
        issueViewModel.IssueDetail!.Versions = versionsOfIssue;
        return issueViewModel;
    }

    private async Task<EpicViewModel> ToEpicViewModel(Issue epic)
    {
        await _issueRepository.LoadEntitiesRelationship(epic);
        var epicViewModel = _mapper.Map<EpicViewModel>(epic);
        var labelsOfEpic = await _labelRepository.GetByIssueId(epic.Id);
        var versionsOfEpic = await _versionRepository.GetStatusViewModelsByIssueId(epic.Id);

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
        epicViewModel.IssueDetail!.Labels = labelsOfEpic;
        epicViewModel.IssueDetail!.Versions = versionsOfEpic;
        return epicViewModel;
    }

    private async Task<GetIssuesByEpicIdViewModel> ToGetIssuesByEpicIdViewModel(Issue epic, IReadOnlyCollection<Issue> childIssues)
    {
        var epicViewModel = _mapper.Map<EpicViewModel>(epic);
        epicViewModel.ChildIssues = await ToIssueViewModels(childIssues);

        if (epic.ProjectId is Guid projectId)
        {
            var backlog = await _backlogRepository.GetBacklog(projectId);
            var issueForBacklog = childIssues.Any() ? childIssues.Where(ci => ci.BacklogId == backlog.Id).ToList() : new List<Issue>();
            var issueViewModels = await ToIssueViewModels(issueForBacklog);
            backlog.Issues = issueViewModels.ToList();

            var sprints = await _sprintRepository.GetSprintByProjectId(projectId);
            if (sprints.Any())
            {
                foreach (var sprint in sprints)
                {
                    var issues = childIssues.Any() ? childIssues.Where(ci => ci.SprintId == sprint.Id).ToList() : new List<Issue>();
                    issueViewModels = await ToIssueViewModels(issues);
                    sprint.Issues = issueViewModels.ToList();
                }
            }

            return new GetIssuesByEpicIdViewModel()
            {
                Sprints = sprints,
                Backlog = backlog,
                Epics = epicViewModel
            };
        }
        else
        {
            return new GetIssuesByEpicIdViewModel()
            {
                Sprints = new List<SprintViewModel>(),
                Backlog = new BacklogViewModel(),
                Epics = epicViewModel
            };
        }
    }

    private async Task<IReadOnlyCollection<ChildIssueViewModel>> ToChildIssueViewModels(IReadOnlyCollection<Issue> issues)
    {
        var childIssueViewModels = new List<ChildIssueViewModel>();
        if (issues.Any())
        {
            foreach (var issue in issues)
            {
                var childIssueViewModel = await ToChildIssueViewModel(issue);
                childIssueViewModels.Add(childIssueViewModel);
            }
        }
        return childIssueViewModels.AsReadOnly();
    }

    private async Task<ChildIssueViewModel> ToChildIssueViewModel(Issue childIssue)
    {
        await _issueRepository.LoadAttachments(childIssue);
        await _issueRepository.LoadIssueDetail(childIssue);
        await _issueRepository.LoadIssueType(childIssue);
        await _issueRepository.LoadStatus(childIssue);
        var labelsOfChildIssue = await _labelRepository.GetByIssueId(childIssue.Id);
        var versionsOfChildIssue = await _versionRepository.GetStatusViewModelsByIssueId(childIssue.Id);

        var childIssueViewModel = _mapper.Map<ChildIssueViewModel>(childIssue);

        if (childIssue.IssueDetail is not null)
        {
            var issueDetail = _mapper.Map<IssueDetailViewModel>(childIssue.IssueDetail);
            childIssueViewModel.IssueDetail = issueDetail;
        }
        if (childIssue.Attachments is not null && childIssue.Attachments.Any())
        {
            var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(childIssue.Attachments);
            childIssueViewModel.Attachments = attachments;
        }
        if (childIssue.IssueType is not null)
        {
            var issueType = _mapper.Map<IssueTypeViewModel>(childIssue.IssueType);
            childIssueViewModel.IssueType = issueType;
        }
        if (childIssue.Status is not null)
        {
            var status = _mapper.Map<StatusViewModel>(childIssue.Status);
            childIssueViewModel.Status = status;
        }
        if (childIssue.ParentId is Guid parentId)
        {
            childIssueViewModel.ParentName = await _issueRepository.GetParentName(parentId);
        }
        childIssueViewModel.IssueDetail!.Labels = labelsOfChildIssue;
        childIssueViewModel.IssueDetail!.Versions = versionsOfChildIssue;
        return childIssueViewModel;
    }

    private async Task<(IReadOnlyCollection<Guid>, UserNotificationViewModel)> DetachUpdateField(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto)
    {
        var senderName = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
        var projectName = await _issueRepository.GetProjectNameOfIssue(issue.Id);
        var projectId = await _issueRepository.GetProjectIdOfIssue(issue.Id);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var projectCode = await _issueRepository.GetProjectCodeOfIssue(issue.Id);

        var issueEditedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.IssueEditedName).FirstOrDefault();
        var someoneAssignedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.SomeoneAssignedToAIssueName).FirstOrDefault();
        var issueDeletedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.IssueDeletedName).FirstOrDefault();
        var issueMovedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.IssueMovedName).FirstOrDefault();

        var issueHistories = new List<IssueHistory>();
        IReadOnlyCollection<Guid> userIds = new List<Guid>();

        if (!string.IsNullOrWhiteSpace(updateIssueDto.Name))
        {
            await ChangeNameIssue(issue, updateIssueDto, issueHistories, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (!string.IsNullOrWhiteSpace(updateIssueDto.Description))
        {
            ChangeDescriptionIssue(issue, updateIssueDto, issueHistories);
        }
        else if (updateIssueDto.ParentId is Guid parentId)
        {
            await ChangeParentIssue(issue, updateIssueDto, issueHistories, parentId, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (updateIssueDto.StatusId is Guid newStatusId)
        {
            var isComplete = await _statusRepository.CheckStatusBelongDone(newStatusId);
            if (isComplete)
            {
                issue.CompleteDate = DateTime.Now;
            }
            await ChangeStatusIssue(issue, updateIssueDto, issueHistories, newStatusId, senderName, projectName, issueMovedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueMovedEvent);
        }
        else if (updateIssueDto.PriorityId is Guid newPriorityId)
        {
            await ChangePriorityIssue(issue, updateIssueDto, issueHistories, newPriorityId, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (
            updateIssueDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is not 0
            || updateIssueDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is 0
            || updateIssueDto.StoryPointEstimate is 0 && issueDetail.StoryPointEstimate is not 0
            )
        {
            await ChangeSPEIssue(issue, issueDetail, updateIssueDto, issueHistories, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (updateIssueDto.ReporterId is Guid reporterId)
        {
            await ChangeReporterIssue(issue, issueDetail, updateIssueDto, issueHistories, reporterId, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (updateIssueDto.StartDate != DateTime.MinValue)
        {
            await ChangeStartDateIssue(issue, updateIssueDto, issueHistories, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (updateIssueDto.DueDate != DateTime.MinValue)
        {
            await ChangeDueDateIssue(issue, updateIssueDto, issueHistories, senderName, projectName, issueEditedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, issueEditedEvent);
        }
        else if (updateIssueDto.AssigneeId != Guid.Empty)
        {
            await ChangeAssigneeIssue(issue, issueDetail, updateIssueDto, issueHistories, senderName, projectName, someoneAssignedEvent, projectCode);
            userIds = await GetUserIdsByNotificationConfig(issue.Id, someoneAssignedEvent);
        }

        _issueHistoryRepository.AddRange(issueHistories);

        var userNotifications = AddUserNotifications(issueHistories, issue.Id, userIds);
        _userNotificationRepository.AddRange(userNotifications);

        await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

        var userNotificationViewModel = await _userNotificationRepository.ToUserNotificationViewMode(userNotifications.Select(un => un.Id).FirstOrDefault());
        return (userIds, userNotificationViewModel!);
    }

    private async Task ChangeNameIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        var updatedTheSumaryHis = new IssueHistory()
        {
            Name = IssueConstants.Summary_IssueHistoryName,
            Content = $"{issue.Name} to {updateIssueDto.Name}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };

        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        issueHistories.Add(updatedTheSumaryHis);

        var changeNameIssueEmailContentDto = new ChangeNameIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
        {
            FromName = issue.Name,
            ToName = updateIssueDto.Name ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeNameIssueContent(changeNameIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

        if (notificationEventViewModel is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
        }

        issue.Name = updateIssueDto.Name ?? string.Empty;
    }

    private static void ChangeDescriptionIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories)
    {
        var updatedTheDescriptionHis = new IssueHistory()
        {
            Name = IssueConstants.Description_IssueHistoryName,
            Content = string.IsNullOrWhiteSpace(issue.Description) ? $"{IssueConstants.None_IssueHistoryContent} to {updateIssueDto.Description}" : $"{issue.Description} to {updateIssueDto.Description}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        issueHistories.Add(updatedTheDescriptionHis);

        issue.Description = updateIssueDto.Description;
    }

    private async Task ChangeParentIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid parentId, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        string? oldParentName = issue.ParentId is not null ? await _issueRepository.GetNameOfIssue((Guid)issue.ParentId) : IssueConstants.None_IssueHistoryContent;
        string? newParentName = await _issueRepository.GetNameOfIssue(parentId);
        var changedTheParentHis = new IssueHistory()
        {
            Name = IssueConstants.Parent_IssueHistoryName,
            Content = $"{oldParentName} to {newParentName}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        issueHistories.Add(changedTheParentHis);

        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var changeParentIssueEmailContentDto = new ChangeParentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
        {
            FromParentName = oldParentName ?? string.Empty,
            ToParentName = newParentName ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeParentIssueContent(changeParentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

        if (notificationEventViewModel is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
        }

        issue.ParentId = parentId;
    }

    private async Task ChangeAssigneeIssue(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        var changedTheAssigneeHis = new IssueHistory()
        {
            Name = IssueConstants.Assignee_IssueHistoryName,

            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, string.Empty, projectCode, issue.Id);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var changeAssigneeIssueEmailContentDto = new ChangeAssigneeIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl);

        if (updateIssueDto.AssigneeId is Guid newAssigneeId && issueDetail.AssigneeId is Guid oldAssigneeId)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = oldAssigneeId,
                To = newAssigneeId,
            }.ToJson();
            issueHistories.Add(changedTheAssigneeHis);

            var fromAssigneeName = await _userManager.Users.Where(u => u.Id == oldAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

            var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

            changeAssigneeIssueEmailContentDto.FromAssigneeName = fromAssigneeName ?? string.Empty;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = toAssigneeName ?? string.Empty;

            string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);

            buidEmailTemplateBaseDto.EmailContent = emailContent;

            if (notificationEventViewModel is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
            }

            issueDetail.AssigneeId = newAssigneeId;
        }

        else if (updateIssueDto.AssigneeId is Guid newAssigneeId1 && issueDetail.AssigneeId is null)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = null,
                To = newAssigneeId1,
            }.ToJson();

            issueHistories.Add(changedTheAssigneeHis);

            var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId1).Select(u => u.Name).FirstOrDefaultAsync();

            changeAssigneeIssueEmailContentDto.FromAssigneeName = IssueConstants.Unassigned_IssueHistoryContent;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = toAssigneeName ?? string.Empty;

            var emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);

            buidEmailTemplateBaseDto.EmailContent = emailContent;


            if (notificationEventViewModel is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
            }

            issueDetail.AssigneeId = newAssigneeId1;
        }

        else if (updateIssueDto.AssigneeId is null && issueDetail.AssigneeId is Guid oldAssigneeId1)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = oldAssigneeId1,
                To = null,
            }.ToJson();

            issueHistories.Add(changedTheAssigneeHis);

            var fromAssigneeName = await _userManager.Users.Where(u => u.Id == oldAssigneeId1).Select(u => u.Name).FirstOrDefaultAsync();

            changeAssigneeIssueEmailContentDto.FromAssigneeName = fromAssigneeName ?? string.Empty;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = IssueConstants.Unassigned_IssueHistoryContent;

            string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);
            buidEmailTemplateBaseDto.EmailContent = emailContent;

            if (notificationEventViewModel is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
            }

            issueDetail.AssigneeId = null;
        }
    }

    private async Task ChangeStatusIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid newStatusId, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        if (issue.StatusId is Guid oldStatusId)
        {
            string? newStatusName = await _statusRepository.GetNameOfStatus(newStatusId);
            string? oldStatusName = await _statusRepository.GetNameOfStatus(oldStatusId);
            var changedTheStatusHis = new IssueHistory()
            {
                Name = IssueConstants.Status_IssueHistoryName,
                Content = $"{oldStatusName} to {newStatusName}",
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };
            issueHistories.Add(changedTheStatusHis);
            var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

            var changeStatusIssueEmailContentDto = new ChangeStatusIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
            {
                FromStatusName = oldStatusName ?? string.Empty,
                ToStatusName = newStatusName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeStatusIssueContent(changeStatusIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

            if (notificationEventViewModel is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
            }
        }

        issue.StatusId = newStatusId;
    }

    private async Task ChangePriorityIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid newPriorityId, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        if (issue.PriorityId is Guid oldPriorityId)
        {
            string? newPriorityName = await _priorityRepository.GetNameOfPriority(newPriorityId);
            string? oldPriorityName = await _priorityRepository.GetNameOfPriority(oldPriorityId);
            var changedThePriorityHis = new IssueHistory()
            {
                Name = IssueConstants.Priority_IssueHistoryName,
                Content = $"{oldPriorityName} to {newPriorityName}",
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };

            issueHistories.Add(changedThePriorityHis);
            var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

            var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
            {
                FromPriorityName = oldPriorityName ?? string.Empty,
                ToPriorityName = newPriorityName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

            if (notificationEventViewModel is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
            }
        }
        else
        {
            string? newPriorityName = await _priorityRepository.GetNameOfPriority(newPriorityId);
            var changedThePriorityHis = new IssueHistory()
            {
                Name = IssueConstants.Priority_IssueHistoryName,
                Content = $"{IssueConstants.None_IssueHistoryContent} to {newPriorityName}",
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };

            issueHistories.Add(changedThePriorityHis);
            var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

            var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
            {
                FromPriorityName = IssueConstants.None_IssueHistoryContent,
                ToPriorityName = newPriorityName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

            if (notificationEventViewModel is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
            }
        }

        issue.PriorityId = newPriorityId;
    }

    private async Task ChangeSPEIssue(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        var updatedTheSPEHis = new IssueHistory()
        {
            Name = IssueConstants.StoryPointEstimate_IssueHistoryName,
            Content = $"{issueDetail.StoryPointEstimate} to {updateIssueDto.StoryPointEstimate}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        issueHistories.Add(updatedTheSPEHis);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var changeSPEIssueEmailContentDto = new ChangeSPEIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
        {
            FromSPEName = issueDetail.StoryPointEstimate.ToString(),
            ToSPEName = updateIssueDto.StoryPointEstimate?.ToString() ?? "0",
        };

        string emailContent = EmailContentConstants.ChangeSPEIssueContent(changeSPEIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

        if (notificationEventViewModel is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
        }

        issueDetail.StoryPointEstimate = updateIssueDto.StoryPointEstimate ?? 0;
    }

    private async Task ChangeReporterIssue(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid reporterId, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        var updatedTheReporterHis = new IssueHistory()
        {
            Name = IssueConstants.Reporter_IssueHistoryName,
            Content = new ReporterFromTo
            {
                From = issueDetail.ReporterId,
                To = reporterId
            }.ToJson(),
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        issueHistories.Add(updatedTheReporterHis);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var fromReporterName = await _userManager.Users.Where(u => u.Id == issueDetail.ReporterId).Select(u => u.Name).FirstOrDefaultAsync();

        var toReporterName = await _userManager.Users.Where(u => u.Id == reporterId).Select(u => u.Name).FirstOrDefaultAsync();

        var changeReporterIssueEmailContentDto = new ChangeReporterIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
        {
            FromReporterName = fromReporterName ?? string.Empty,
            ToReporterName = toReporterName ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeReporterIssueContent(changeReporterIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

        if (notificationEventViewModel is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
        }

        issueDetail.ReporterId = reporterId;
    }

    private async Task ChangeStartDateIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        var updatedTheStartDateHis = new IssueHistory()
        {
            Name = IssueConstants.StartDate_EpicHistoryName,
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var changeStartDateIssueEmailContentDto = new ChangeStartDateIssueEmailContentDto(senderName, issue.CreationTime, avatarUrl);

        if (updateIssueDto.StartDate is DateTime newStartDate && issue.StartDate is null)
        {
            updatedTheStartDateHis.Content = $"{IssueConstants.None_IssueHistoryContent} to {newStartDate:MMM dd, yyyy}";

            changeStartDateIssueEmailContentDto.FromStartDate = IssueConstants.None_IssueHistoryContent;
            changeStartDateIssueEmailContentDto.ToStartDate = newStartDate.ToString("dd/MMM/yy");

            issue.StartDate = newStartDate;
        }
        else if (updateIssueDto.StartDate is DateTime newStartDate1 && issue.StartDate is DateTime oldStartDate1)
        {
            updatedTheStartDateHis.Content = $"{oldStartDate1:MMM dd, yyyy} to {newStartDate1:MMM dd, yyyy}";

            changeStartDateIssueEmailContentDto.FromStartDate = oldStartDate1.ToString("dd/MMM/yy");
            changeStartDateIssueEmailContentDto.ToStartDate = newStartDate1.ToString("dd/MMM/yy");

            issue.StartDate = newStartDate1;
        }
        else if (updateIssueDto.StartDate is null && issue.StartDate is DateTime oldStartDate2)
        {
            updatedTheStartDateHis.Content = $"{oldStartDate2:MMM dd, yyyy} to {IssueConstants.None_IssueHistoryContent}";

            changeStartDateIssueEmailContentDto.FromStartDate = oldStartDate2.ToString("dd/MMM/yy");
            changeStartDateIssueEmailContentDto.ToStartDate = IssueConstants.None_IssueHistoryContent;

            issue.StartDate = null;
            updateIssueDto.StartDate = null;
        }

        string emailContent = EmailContentConstants.ChangeStartDateIssueContent(changeStartDateIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

        if (notificationEventViewModel is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
        }

        issueHistories.Add(updatedTheStartDateHis);
    }

    private async Task ChangeDueDateIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName, NotificationEventViewModel? notificationEventViewModel, string projectCode)
    {
        var updatedTheDueDateHis = new IssueHistory()
        {
            Name = IssueConstants.DueDate_EpicHistoryName,
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        var avatarUrl = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var changeDueDateIssueEmailContentDto = new ChangeDueDateIssueEmailContentDto(senderName, issue.CreationTime, avatarUrl);

        if (updateIssueDto.DueDate is DateTime newDueDate && issue.DueDate is null)
        {
            updatedTheDueDateHis.Content = $"{IssueConstants.None_IssueHistoryContent} to {newDueDate:MMM dd, yyyy}";

            changeDueDateIssueEmailContentDto.FromDueDate = IssueConstants.None_IssueHistoryContent;
            changeDueDateIssueEmailContentDto.ToDueDate = newDueDate.ToString("dd/MMM/yy");

            issue.DueDate = newDueDate;
        }
        else if (updateIssueDto.DueDate is DateTime newDueDate1 && issue.DueDate is DateTime oldDueDate1)
        {
            updatedTheDueDateHis.Content = $"{oldDueDate1:MMM dd, yyyy} to {newDueDate1:MMM dd, yyyy}";

            changeDueDateIssueEmailContentDto.FromDueDate = oldDueDate1.ToString("dd/MMM/yy");
            changeDueDateIssueEmailContentDto.ToDueDate = newDueDate1.ToString("dd/MMM/yy");

            issue.DueDate = newDueDate1;

        }
        else if (updateIssueDto.DueDate is null && issue.DueDate is DateTime oldDueDate2)
        {
            updatedTheDueDateHis.Content = $"{oldDueDate2:MMM dd, yyyy} to {IssueConstants.None_IssueHistoryContent}";

            changeDueDateIssueEmailContentDto.FromDueDate = oldDueDate2.ToString("dd/MMM/yy");
            changeDueDateIssueEmailContentDto.ToDueDate = IssueConstants.None_IssueHistoryContent;

            issue.DueDate = null;
            updateIssueDto.DueDate = null;
        }
        string emailContent = EmailContentConstants.ChangeDueDateIssueContent(changeDueDateIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

        if (notificationEventViewModel is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto, notificationEventViewModel);
        }

        issueHistories.Add(updatedTheDueDateHis);

    }

    private async Task AddVersionOrLabel(Issue issue, UpdateEpicDto updateIssueDto)
    {
        if (updateIssueDto.VersionIds is not null && updateIssueDto.VersionIds.Any())
        {
            var removedVersionIssues = await _versionRepository.GetVersionIssuesByIssueId(issue.Id);
            _versionRepository.RemoveRange(removedVersionIssues);

            var versionIssues = new List<VersionIssue>();
            foreach (var versionId in updateIssueDto.VersionIds)
            {
                var versionIssue = new VersionIssue()
                {
                    VersionId = versionId,
                    IssueId = issue.Id,
                };
                versionIssues.Add(versionIssue);
            }

            _versionRepository.AddRange(versionIssues);
            await _labelRepository.UnitOfWork.SaveChangesAsync();
        }
        else if (updateIssueDto.VersionIds is not null && updateIssueDto.VersionIds.Count == 0)
        {
            var removedVersionIssues = await _versionRepository.GetVersionIssuesByIssueId(issue.Id);
            _versionRepository.RemoveRange(removedVersionIssues);
            await _labelRepository.UnitOfWork.SaveChangesAsync();
        }
        if (updateIssueDto.LabelIds is not null && updateIssueDto.LabelIds.Any())
        {
            var removedLabelIssues = await _labelRepository.GetLabelIssuesByIssueId(issue.Id);
            _labelRepository.RemoveRange(removedLabelIssues);

            var labelIssues = new List<LabelIssue>();
            foreach (var labelId in updateIssueDto.LabelIds)
            {
                var labelIssue = new LabelIssue()
                {
                    LabelId = labelId,
                    IssueId = issue.Id,
                };
                labelIssues.Add(labelIssue);
            }

            _labelRepository.AddRange(labelIssues);
            await _labelRepository.UnitOfWork.SaveChangesAsync();
        }
        else if (updateIssueDto.LabelIds is not null && updateIssueDto.LabelIds.Count == 0)
        {
            var removedLabelIssues = await _labelRepository.GetLabelIssuesByIssueId(issue.Id);
            _labelRepository.RemoveRange(removedLabelIssues);
            await _labelRepository.UnitOfWork.SaveChangesAsync();
        }
    }

    private static IReadOnlyCollection<UserNotification> AddUserNotifications(IReadOnlyCollection<IssueHistory> issueHistories, Guid issueId, IReadOnlyCollection<Guid> userIds)
    {
        var userNotifications = new List<UserNotification>();
        if (issueHistories.Any())
        {
            foreach (var issueHistory in issueHistories)
            {
                foreach (var userId in userIds)
                {
                    if (userId != Guid.Empty)
                    {
                        var userNotification = new UserNotification()
                        {
                            Name = issueHistory.Name,
                            CreatorUserId = issueHistory.CreatorUserId,
                            IssueId = issueId,
                            IsRead = false,
                            UserId = userId,
                        };
                        userNotifications.Add(userNotification);
                    }
                }
            }
        }

        return userNotifications;
    }

    private async Task<IReadOnlyCollection<Guid>> GetUserIdsByNotificationConfig(Guid issueId, NotificationEventViewModel? notificationEventViewModel)
    {
        if (notificationEventViewModel is null)
        {
            return new List<Guid>();
        }

        var userIds = new List<Guid>();
        if (notificationEventViewModel.AllWatcher)
        {
            var watcherIds = await _issueRepository.GetAllWatcherOfIssue(issueId);
            userIds.AddRange(watcherIds!);
        }
        if (notificationEventViewModel.CurrentAssignee)
        {
            var currentAssigneeId = await _issueDetailRepository.GetCurrentAssignee(issueId);
            if (currentAssigneeId is Guid id && id != Guid.Empty)
            {
                userIds.Add(id);
            }

        }
        if (notificationEventViewModel.Reporter)
        {
            var reporterId = await _issueDetailRepository.GetReporter(issueId);
            userIds.Add(reporterId);
        }
        if (notificationEventViewModel.ProjectLead)
        {
            var projectLeadId = await _issueRepository.GetProjectLeadIdOfIssue(issueId);
            userIds.Add(projectLeadId);
        }

        return userIds.Distinct().ToList();
    }
    #endregion

    public async Task<EpicViewModel> AddIssueToEpic(Guid issueId, Guid epicId)
    {
        var issue = await _issueRepository.Get(issueId);
        if (issue is null)
        {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new ArgumentNullException(nameof(issue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
        }
        issue.ParentId = epicId;
        _issueRepository.Update(issue);
        await _issueRepository.UnitOfWork.SaveChangesAsync();
        var epic = await _issueRepository.Get(epicId);
        return await ToEpicViewModel(epic);
    }

    public async Task<RealtimeNotificationViewModel> CreateEpic(CreateEpicDto createEpicDto)
    {
        var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createEpicDto.ProjectId);
        int issueIndex = projectConfiguration.IssueCode + 1;
        var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createEpicDto.ProjectId);
        var creatorUser = await _userManager.FindByIdAsync(createEpicDto.CreatorUserId.ToString());
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(createEpicDto.ProjectId);

        var issueType = await _issueTypeRepository.GetEpic(createEpicDto.ProjectId);

        var issue = new Issue()
        {
            Name = createEpicDto.Name,
            IssueTypeId = issueType.Id,
            Code = $"{projectConfiguration.Code}-{issueIndex}",
            ProjectId = createEpicDto.ProjectId,
            StatusId = createTransition.ToStatusId,
            Watcher = new()
        };

        if (creatorUser is not null && issue.Watcher is not null && issue.Watcher.Users is not null)
        {
            var user = new User()
            {
                Identity = creatorUser.Id,
                Name = creatorUser.Name,
                Email = creatorUser.Email!
            };
            issue.Watcher.Users.Add(user);
        }

        _issueRepository.Add(issue);
        await _issueRepository.UnitOfWork.SaveChangesAsync();
        var epicViewModel = _mapper.Map<EpicViewModel>(issue);

        var issueDetail = new IssueDetail()
        {
            ReporterId = createEpicDto.CreatorUserId,
            StoryPointEstimate = 0,
            Label = string.Empty,
            IssueId = issue.Id,
        };

        _issueDetailRepository.Add(issueDetail);
        await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

        var issueHis = new IssueHistory
        {
            Name = "created the Issue",
            CreatorUserId = createEpicDto.CreatorUserId,
            IssueId = issue.Id,
        };

        _issueHistoryRepository.Add(issueHis);
        await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

        projectConfiguration.IssueCode = issueIndex;
        _projectConfigurationRepository.Update(projectConfiguration);
        await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

        var issueCreatedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.IssueCreatedName).FirstOrDefault();

        if (issueCreatedEvent is not null)
        {
            var reporterName = await _userManager.Users.Where(u => u.Id == createEpicDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
            var senderName = await _userManager.Users.Where(u => u.Id == createEpicDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
            var projectName = await _projectRepository.GetProjectName(createEpicDto.ProjectId);
            var projectCode = await _issueRepository.GetProjectCodeOfIssue(issue.Id);
            var avatarUrl = await _userManager.Users.Where(u => u.Id == createEpicDto.CreatorUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

            var createdIssueEmailContentDto = new CreatedIssueEmailContentDto(reporterName, issue.CreationTime, avatarUrl)
            {
                IssueTypeName = await _issueTypeRepository.GetNameOfIssueType(epicViewModel.IssueTypeId) ?? IssueConstants.None_IssueHistoryContent,
                AssigneeName = await _userManager.Users.Where(u => u.Id == issueDetail.AssigneeId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.Unassigned_IssueHistoryContent,
                PriorityName = issue.PriorityId is not null ? await _priorityRepository.GetNameOfPriority((Guid)issue.PriorityId) ?? IssueConstants.None_IssueHistoryContent : IssueConstants.None_IssueHistoryContent,
                AssigneeAvatarUrl = await _userManager.Users.Where(u => u.Id == issueDetail.AssigneeId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar + CoreConstants.AccessTokenAvatar,
            };
            string emailContent = EmailContentConstants.CreatedIssueContent(createdIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: createEpicDto.CreatorUserId, buidEmailTemplateBaseDto, issueCreatedEvent);
        }

        var userIds = await GetUserIdsByNotificationConfig(issue.Id, issueCreatedEvent);
        var userNotifications = AddUserNotifications(
            new List<IssueHistory>()
            {
                issueHis
            },
            issue.Id,
            userIds);
        _userNotificationRepository.AddRange(userNotifications);
        await _userNotificationRepository.UnitOfWork.SaveChangesAsync();

        var userNotificationViewModel = await _userNotificationRepository.ToUserNotificationViewMode(userNotifications.Select(un => un.Id).FirstOrDefault());

        return new RealtimeNotificationViewModel()
        {
            UserIds = userIds,
            Notification = userNotificationViewModel!,
            Epic = await ToEpicViewModel(issue),
        };
    }

    public async Task<GetIssuesByEpicIdViewModel> GetIssuesByEpicId(Guid epicId)
    {
        var epic = await _issueRepository.Get(epicId);
        var childIssues = await _issueRepository.GetChildIssueOfEpic(epicId);
        return await ToGetIssuesByEpicIdViewModel(epic, childIssues);
    }

    public async Task<RealtimeNotificationViewModel> UpdateEpic(Guid id, UpdateEpicDto updateEpicDto)
    {
        var epic = await _issueRepository.Get(id) ?? throw new IssueNullException();
        var issueDetail = await _issueDetailRepository.GetById(id) ?? throw new IssueDetailNullException();

        var (userIds, userNotificationViewModel) = await DetachUpdateField(epic, issueDetail, updateEpicDto);
        await AddVersionOrLabel(epic, updateEpicDto);
        _issueRepository.Update(epic);
        await _issueRepository.UnitOfWork.SaveChangesAsync();
        await _issueDetailRepository.UnitOfWork.SaveChangesAsync();
        return new RealtimeNotificationViewModel()
        {
            UserIds = userIds,
            Notification = userNotificationViewModel,
            Epic = await ToEpicViewModel(epic),
        };
    }

    public async Task<RealtimeNotificationViewModel> DeleteEpic(Guid id, Guid userId)
    {
        var projectId = await _issueRepository.GetProjectIdOfIssue(id);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var issue = await _issueRepository.GetById(id) ?? throw new IssueNullException();

        var issueDeletedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.IssueDeletedName).FirstOrDefault();

        if (issueDeletedEvent is not null)
        {
            var reporterName = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
            var avatarUrl = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

            var deletedIssueEmailContentDto = new DeletedIssueEmailContentDto(reporterName, issue.CreationTime, avatarUrl)
            {
                IssueName = issue.Name,
            };

            string emailContent = EmailContentConstants.DeleteIssueContent(deletedIssueEmailContentDto);
            var senderName = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
            var projectName = await _issueRepository.GetProjectNameOfIssue(issue.Id);
            var projectCode = await _issueRepository.GetProjectCodeOfIssue(issue.Id);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.DeletedIssue, projectName, issue.Code, issue.Name, emailContent, projectCode, issue.Id);

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: userId, buidEmailTemplateBaseDto, issueDeletedEvent);
        }

        var userIds = await GetUserIdsByNotificationConfig(issue.Id, issueDeletedEvent);
        var userNotification = new UserNotification()
        {
            Name = EmailConstants.DeletedIssue,
            CreatorUserId = userId,
            IssueId = issue.Id,
            IsRead = false,
            UserId = userId,
        };
        _userNotificationRepository.Add(userNotification);
        await _userNotificationRepository.UnitOfWork.SaveChangesAsync();

        var userNotificationViewModel = await _userNotificationRepository.ToUserNotificationViewMode(userNotification.Id);

        await _issueRepository.UpdateOneColumnForIssue(id, null, NameColumn.ParentId);
        _issueRepository.Delete(id);
        await _issueRepository.UnitOfWork.SaveChangesAsync();
        return new RealtimeNotificationViewModel()
        {
            Notification = userNotificationViewModel!,
            IssueId = id
        };
    }
}
