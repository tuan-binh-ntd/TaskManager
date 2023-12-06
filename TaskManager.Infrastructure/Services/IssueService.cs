using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class IssueService : IIssueService
{
    private readonly IIssueRepository _issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository;
    private readonly IIssueDetailRepository _issueDetailRepository;
    private readonly IProjectConfigurationRepository _projectConfigurationRepository;
    private readonly IIssueTypeRepository _issueTypeRepository;
    private readonly ITransitionRepository _transitionRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IBacklogRepository _backlogRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository;
    private readonly IEmailSender _emailSender;
    private readonly IStatusRepository _statusRepository;
    private readonly IPriorityRepository _priorityRepository;
    private readonly IVersionRepository _versionRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public IssueService(
        IIssueRepository issueRepository,
        IIssueHistoryRepository issueHistoryRepository,
        IIssueDetailRepository issueDetailRepository,
        IProjectConfigurationRepository projectConfigurationRepository,
        IIssueTypeRepository issueTypeRepository,
        ITransitionRepository transitionRepository,
        UserManager<AppUser> userManager,
        IBacklogRepository backlogRepository,
        ISprintRepository sprintRepository,
        IStatusCategoryRepository statusCategoryRepository,
        IEmailSender emailSender,
        IStatusRepository statusRepository,
        IPriorityRepository priorityRepository,
        IVersionRepository versionRepository,
        ILabelRepository labelRepository,
        IProjectRepository projectRepository,
        IMapper mapper
        )
    {
        _issueRepository = issueRepository;
        _issueHistoryRepository = issueHistoryRepository;
        _issueDetailRepository = issueDetailRepository;
        _projectConfigurationRepository = projectConfigurationRepository;
        _issueTypeRepository = issueTypeRepository;
        _transitionRepository = transitionRepository;
        _userManager = userManager;
        _backlogRepository = backlogRepository;
        _sprintRepository = sprintRepository;
        _statusCategoryRepository = statusCategoryRepository;
        _emailSender = emailSender;
        _statusRepository = statusRepository;
        _priorityRepository = priorityRepository;
        _versionRepository = versionRepository;
        _labelRepository = labelRepository;
        _projectRepository = projectRepository;
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
        var childIssues = await _issueRepository.GetChildIssueOfIssue(issue.Id);
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
        if (childIssues.Any())
        {
            issueViewModel.ChildIssues = await ToChildIssueViewModels(childIssues);
        }
        return issueViewModel;
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
        return childIssueViewModel;
    }

    private async Task<IssueForProjectViewModel> ToIssueForProjectViewModel(Issue issue)
    {
        var childIssues = await _issueRepository.GetChildIssueOfIssue(issue.Id);
        var doneStatusCategory = await _statusCategoryRepository.GetDone() ?? throw new StatusCategoryNullException();
        if (childIssues.Any())
        {
            foreach (var childIssue in childIssues)
            {
                await _issueRepository.LoadStatus(childIssue);
            }

            var doneChildIssuesNum = childIssues.Where(ci => ci.Status!.StatusCategoryId == doneStatusCategory.Id).Count();
            var childIssuesNum = childIssues.Count;

            return new IssueForProjectViewModel()
            {
                Id = issue.Id,
                Name = issue.Name,
                Start = issue.StartDate,
                End = issue.DueDate,
                Type = issue.ProjectId is null ? "task" : "project",
                Project = issue.ProjectId is null ? issue.ParentId : null,
                Progress = (doneChildIssuesNum / childIssuesNum) * 100,
                HideChildren = issue.ProjectId is null ? null : false,
            };
        }
        else
        {
            return new IssueForProjectViewModel()
            {
                Id = issue.Id,
                Name = issue.Name,
                Start = issue.StartDate,
                End = issue.DueDate,
                Type = issue.ProjectId is null ? "task" : "project",
                Project = issue.ProjectId is null ? issue.ParentId : null,
                Progress = 0,
                HideChildren = issue.ProjectId is null ? null : false,
            };
        }
    }

    private async Task<IReadOnlyCollection<IssueForProjectViewModel>> ToIssueForProjectViewModels(IReadOnlyCollection<Issue> issues)
    {
        var issueViewModels = new List<IssueForProjectViewModel>();
        if (issues.Any())
        {
            foreach (var issue in issues)
            {
                var issueViewModel = await ToIssueForProjectViewModel(issue);
                issueViewModels.Add(issueViewModel);
            }
        }
        return issueViewModels.AsReadOnly();
    }

    private async Task DetachUpdateField(Issue issue, IssueDetail issueDetail, UpdateIssueDto updateIssueDto)
    {
        var senderName = await _userManager.Users.Where(u => u.Id == updateIssueDto.ModificationUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
        var projectName = await _issueRepository.GetProjectNameOfIssue(issue.Id);

        var issueHistories = new List<IssueHistory>();
        if (!string.IsNullOrWhiteSpace(updateIssueDto.Name))
        {
            await ChangeNameIssue(issue, updateIssueDto, issueHistories, senderName, projectName);
        }
        else if (!string.IsNullOrWhiteSpace(updateIssueDto.Description))
        {
            ChangeDescriptionIssue(issue, updateIssueDto, issueHistories);
        }
        else if (updateIssueDto.ParentId is Guid parentId)
        {
            await ChangeParentIssue(issue, updateIssueDto, issueHistories, parentId, senderName, projectName);
        }
        else if (updateIssueDto.SprintId is Guid newSprintId)
        {
            await ChangeSprintIssue(issue, updateIssueDto, issueHistories, newSprintId, senderName, projectName);
        }
        else if (updateIssueDto.IssueTypeId is Guid newIssueTypeId)
        {
            await ChangeIssueTypeIssue(issue, updateIssueDto, issueHistories, newIssueTypeId, senderName, projectName);
        }
        else if (updateIssueDto.BacklogId is Guid backlogId)
        {
            await ChangeBacklogIssue(issue, updateIssueDto, issueHistories, backlogId, senderName, projectName);
        }
        else if (updateIssueDto.StatusId is Guid newStatusId)
        {
            await ChangeStatusIssue(issue, updateIssueDto, issueHistories, newStatusId, senderName, projectName);
        }
        else if (updateIssueDto.PriorityId is Guid newPriorityId)
        {
            await ChangePriorityIssue(issue, updateIssueDto, issueHistories, newPriorityId, senderName, projectName);
        }
        else if (
            updateIssueDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is not 0
            || updateIssueDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is 0
            || updateIssueDto.StoryPointEstimate is 0 && issueDetail.StoryPointEstimate is not 0
            )
        {
            await ChangeSPEIssue(issue, issueDetail, updateIssueDto, issueHistories, senderName, projectName);
        }
        else if (updateIssueDto.ReporterId is Guid reporterId)
        {
            await ChangeReporterIssue(issue, issueDetail, updateIssueDto, issueHistories, reporterId, senderName, projectName);
        }

        await ChangeAssigneeIssue(issue, issueDetail, updateIssueDto, issueHistories, senderName, projectName);

        _issueHistoryRepository.AddRange(issueHistories);
        await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();
    }

    private async Task AddWatcher(Issue issue, UpdateIssueDto updateIssueDto)
    {
        if (updateIssueDto.UserIds is not null && updateIssueDto.UserIds.Any())
        {
            foreach (var item in updateIssueDto.UserIds)
            {
                var user = await _userManager.FindByIdAsync(item.ToString());
                if (user is not null && issue.Watcher is not null && issue.Watcher.Users is not null)
                {
                    var watcher = new User()
                    {
                        Identity = user.Id,
                        Name = user.Name,
                        Email = user.Email!
                    };
                    issue.Watcher.Users.Add(watcher);
                }
            }
        }

        if (issue.Watcher is not null && issue.Watcher.Users is not null)
        {
            issue.Watcher.Users = issue.Watcher.Users.DistinctBy(i => i.Identity).ToList();
        }
    }

    private async Task ChangeSprintOrBacklog(Issue issue, UpdateIssueDto updateIssueDto)
    {
        if (updateIssueDto.SprintId is Guid sprintId && updateIssueDto.BacklogId is null)
        {
            var sprint = _sprintRepository.Get(sprintId) ?? throw new SprintNullException();
            issue.SprintId = sprintId;
            issue.BacklogId = null;
            issue.StartDate = sprint.StartDate;
            issue.DueDate = sprint.EndDate;

            var childIssues = await _issueRepository.GetChildIssueOfIssue(issue.Id);
            if (childIssues.Any())
            {

                foreach (var childIssue in childIssues)
                {
                    childIssue.SprintId = sprintId;
                    childIssue.BacklogId = null;
                }
                _issueRepository.UpdateRange(childIssues);
            }
        }

        if (updateIssueDto.SprintId is null && updateIssueDto.BacklogId is Guid backlogId)
        {
            issue.SprintId = null;
            issue.BacklogId = backlogId;

            var childIssues = await _issueRepository.GetChildIssueOfIssue(issue.Id);
            if (childIssues.Any())
            {
                foreach (var childIssue in childIssues)
                {
                    childIssue.SprintId = null;
                    childIssue.BacklogId = backlogId;
                }
                _issueRepository.UpdateRange(childIssues);
            }
        }
    }

    private static void UpdateIssueDetail(IssueDetail issueDetail, UpdateIssueDto updateIssueDto)
    {
        if (updateIssueDto.StoryPointEstimate is int storyPointEstimate)
        {
            issueDetail.StoryPointEstimate = storyPointEstimate;
        }
        if (updateIssueDto.ReporterId is Guid reporterId)
        {
            issueDetail.ReporterId = reporterId;
        }
        if (updateIssueDto.AssigneeId is not null)
        {
            issueDetail.AssigneeId = updateIssueDto.AssigneeId;
        }
    }

    private async Task AddVersionOrLabel(Issue issue, UpdateIssueDto updateIssueDto)
    {
        if (updateIssueDto.VersionId is Guid versionId)
        {
            var versionIssue = new VersionIssue()
            {
                IssueId = issue.Id,
                VersionId = versionId
            };
            _versionRepository.AddVersionIssue(versionIssue);
            await _versionRepository.UnitOfWork.SaveChangesAsync();
        }

        if (updateIssueDto.LabelId is Guid labelId)
        {
            var labelIssue = new LabelIssue()
            {
                LabelId = labelId,
                IssueId = issue.Id,
            };

            _labelRepository.AddLabelIssue(labelIssue);
            await _labelRepository.UnitOfWork.SaveChangesAsync();
        }
    }

    private async Task ChangeNameIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
    {
        var updatedTheSumaryHis = new IssueHistory()
        {
            Name = IssueConstants.Summary_IssueHistoryName,
            Content = $"{issue.Name} to {updateIssueDto.Name}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };

        issueHistories.Add(updatedTheSumaryHis);

        var changeNameIssueEmailContentDto = new ChangeNameIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
        {
            FromName = issue.Name,
            ToName = updateIssueDto.Name ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeNameIssueContent(changeNameIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
        {
            SenderName = senderName,
            ActionName = EmailConstants.MadeOneUpdate,
            ProjectName = projectName,
            IssueCode = issue.Code,
            IssueName = issue.Name,
            EmailContent = emailContent,
        };

        await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);

        issue.Name = updateIssueDto.Name ?? string.Empty;
    }

    private static void ChangeDescriptionIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories)
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

    private async Task ChangeParentIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid parentId, string senderName, string projectName)
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

        var changeParentIssueEmailContentDto = new ChangeParentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
        {
            FromParentName = oldParentName ?? string.Empty,
            ToParentName = newParentName ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeParentIssueContent(changeParentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
        {
            SenderName = senderName,
            ActionName = EmailConstants.MadeOneUpdate,
            ProjectName = projectName,
            IssueCode = issue.Code,
            IssueName = issue.Name,
            EmailContent = emailContent,
        };

        await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);

        issue.ParentId = parentId;
    }

    private async Task ChangeSprintIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid newSprintId, string senderName, string projectName)
    {
        if (issue.SprintId is Guid oldSprintId)
        {
            string? oldSprintName = await _sprintRepository.GetNameOfSprint(oldSprintId);
            string? newSprintName = await _sprintRepository.GetNameOfSprint(newSprintId);
            var changedTheParentHis = new IssueHistory()
            {
                Name = IssueConstants.Parent_IssueHistoryName,
                Content = $"{oldSprintName} to {newSprintName}",
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };
            issueHistories.Add(changedTheParentHis);

            var changeSprintIssueEmailContentDto = new ChangeSprintIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromSprintName = oldSprintName ?? string.Empty,
                ToSprintName = newSprintName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeSprintIssueContent(changeSprintIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
        }
        else if (issue.SprintId is null)
        {
            string? newSprintName = await _sprintRepository.GetNameOfSprint(newSprintId);
            var changedTheParentHis = new IssueHistory()
            {
                Name = IssueConstants.Parent_IssueHistoryName,
                Content = $"{IssueConstants.None_IssueHistoryContent} to {newSprintName}",
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };
            issueHistories.Add(changedTheParentHis);

            var changeSprintIssueEmailContentDto = new ChangeSprintIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromSprintName = string.Empty,
                ToSprintName = newSprintName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeSprintIssueContent(changeSprintIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
        }

        issue.SprintId = newSprintId;
    }

    private async Task ChangeIssueTypeIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid newIssueTypeId, string senderName, string projectName)
    {
        string? newIssueTypeName = await _issueTypeRepository.GetNameOfIssueType(newIssueTypeId);
        string? oldIssueTypeName = await _issueTypeRepository.GetNameOfIssueType(issue.IssueTypeId);
        var updatedTheIssueTypeHis = new IssueHistory()
        {
            Name = IssueConstants.IssueType_IssueHistoryName,
            Content = $"{oldIssueTypeName} to {newIssueTypeName}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        issueHistories.Add(updatedTheIssueTypeHis);

        var changeIssueTypeIssueEmailContentDto = new ChangeIssueTypeIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
        {
            FromIssueTypeName = oldIssueTypeName ?? string.Empty,
            ToIssueTypeName = newIssueTypeName ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeIssueTypeIssueContent(changeIssueTypeIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
        {
            SenderName = senderName,
            ActionName = EmailConstants.MadeOneUpdate,
            ProjectName = projectName,
            IssueCode = issue.Code,
            IssueName = issue.Name,
            EmailContent = emailContent,
        };

        await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);

        issue.IssueTypeId = newIssueTypeId;
    }

    private async Task ChangeBacklogIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid backlogId, string senderName, string projectName)
    {
        if (issue.SprintId is Guid oldSprintId)
        {
            string? oldSprintName = await _sprintRepository.GetNameOfSprint(oldSprintId);
            var changedTheBacklogHis = new IssueHistory()
            {
                Name = IssueConstants.Sprint_IssueHistoryName,
                Content = $"{oldSprintName} to {IssueConstants.None_IssueHistoryContent}",
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };
            issueHistories.Add(changedTheBacklogHis);

            var changeSprintIssueEmailContentDto = new ChangeSprintIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromSprintName = oldSprintName ?? string.Empty,
                ToSprintName = string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeSprintIssueContent(changeSprintIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
        }
        issue.BacklogId = backlogId;
    }

    private async Task ChangeAssigneeIssue(Issue issue, IssueDetail issueDetail, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
    {
        if (updateIssueDto.AssigneeId is Guid newAssigneeId)
        {
            if (issueDetail.AssigneeId is Guid oldAssigneeId)
            {
                var changedTheAssigneeHis = new IssueHistory()
                {
                    Name = IssueConstants.Assignee_IssueHistoryName,
                    Content = new AssigneeFromTo
                    {
                        From = oldAssigneeId,
                        To = newAssigneeId,
                    }.ToJson(),
                    CreatorUserId = updateIssueDto.ModificationUserId,
                    IssueId = issue.Id,
                };
                issueHistories.Add(changedTheAssigneeHis);

                var fromAssigneeName = await _userManager.Users.Where(u => u.Id == oldAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

                var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

                var changeAssigneeIssueEmailContentDto = new ChangeAssigneeIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
                {
                    FromAssigneeName = fromAssigneeName ?? string.Empty,
                    ToAssigneeName = toAssigneeName ?? string.Empty,
                };

                string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);

                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
                {
                    SenderName = senderName,
                    ActionName = EmailConstants.MadeOneUpdate,
                    ProjectName = projectName,
                    IssueCode = issue.Code,
                    IssueName = issue.Name,
                    EmailContent = emailContent,
                };

                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
            }
            else if (issueDetail.AssigneeId is null)
            {
                var changedTheAssigneeHis = new IssueHistory()
                {
                    Name = IssueConstants.Assignee_IssueHistoryName,
                    Content = new AssigneeFromTo
                    {
                        From = null,
                        To = newAssigneeId,
                    }.ToJson(),
                    CreatorUserId = updateIssueDto.ModificationUserId,
                    IssueId = issue.Id,
                };
                issueHistories.Add(changedTheAssigneeHis);

                var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

                var changeAssigneeIssueEmailContentDto = new ChangeAssigneeIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
                {
                    FromAssigneeName = IssueConstants.Unassigned_IssueHistoryContent,
                    ToAssigneeName = toAssigneeName ?? string.Empty,
                };

                string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);

                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
                {
                    SenderName = senderName,
                    ActionName = EmailConstants.MadeOneUpdate,
                    ProjectName = projectName,
                    IssueCode = issue.Code,
                    IssueName = issue.Name,
                    EmailContent = emailContent,
                };

                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
            }

            issueDetail.AssigneeId = newAssigneeId;
        }
        else if (updateIssueDto.AssigneeId is null && issueDetail.AssigneeId is Guid oldAssigneeId)
        {
            var changedTheAssigneeHis = new IssueHistory()
            {
                Name = IssueConstants.Assignee_IssueHistoryName,
                Content = new AssigneeFromTo
                {
                    From = oldAssigneeId,
                    To = null,
                }.ToJson(),
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };
            issueHistories.Add(changedTheAssigneeHis);

            var fromAssigneeName = await _userManager.Users.Where(u => u.Id == oldAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

            var changeAssigneeIssueEmailContentDto = new ChangeAssigneeIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromAssigneeName = fromAssigneeName ?? string.Empty,
                ToAssigneeName = IssueConstants.Unassigned_IssueHistoryContent,
            };

            string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);

            issueDetail.AssigneeId = null;
        }
    }

    private async Task ChangeStatusIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid newStatusId, string senderName, string projectName)
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

            var changeStatusIssueEmailContentDto = new ChangeStatusIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromStatusName = oldStatusName ?? string.Empty,
                ToStatusName = newStatusName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeStatusIssueContent(changeStatusIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
        }

        issue.StatusId = newStatusId;
    }

    private async Task ChangePriorityIssue(Issue issue, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid newPriorityId, string senderName, string projectName)
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

            var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromPriorityName = oldPriorityName ?? string.Empty,
                ToPriorityName = newPriorityName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
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

            var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                FromPriorityName = IssueConstants.None_IssueHistoryContent,
                ToPriorityName = newPriorityName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);
        }

        issue.PriorityId = newPriorityId;
    }

    private async Task ChangeSPEIssue(Issue issue, IssueDetail issueDetail, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
    {
        var updatedTheSPEHis = new IssueHistory()
        {
            Name = IssueConstants.StoryPointEstimate_IssueHistoryName,
            Content = $"{issueDetail.StoryPointEstimate} to {updateIssueDto.StoryPointEstimate}",
            CreatorUserId = updateIssueDto.ModificationUserId,
            IssueId = issue.Id,
        };
        issueHistories.Add(updatedTheSPEHis);

        var changeSPEIssueEmailContentDto = new ChangeSPEIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
        {
            FromSPEName = issueDetail.StoryPointEstimate.ToString(),
            ToSPEName = updateIssueDto.StoryPointEstimate?.ToString() ?? "0",
        };

        string emailContent = EmailContentConstants.ChangeSPEIssueContent(changeSPEIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
        {
            SenderName = senderName,
            ActionName = EmailConstants.MadeOneUpdate,
            ProjectName = projectName,
            IssueCode = issue.Code,
            IssueName = issue.Name,
            EmailContent = emailContent,
        };

        await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);

        issueDetail.StoryPointEstimate = updateIssueDto.StoryPointEstimate ?? 0;
    }

    private async Task ChangeReporterIssue(Issue issue, IssueDetail issueDetail, UpdateIssueDto updateIssueDto, List<IssueHistory> issueHistories, Guid reporterId, string senderName, string projectName)
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

        var fromReporterName = await _userManager.Users.Where(u => u.Id == issueDetail.ReporterId).Select(u => u.Name).FirstOrDefaultAsync();

        var toReporterName = await _userManager.Users.Where(u => u.Id == reporterId).Select(u => u.Name).FirstOrDefaultAsync();

        var changeReporterIssueEmailContentDto = new ChangeReporterIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
        {
            ReporterName = senderName,
            FromReporterName = fromReporterName ?? string.Empty,
            ToReporterName = toReporterName ?? string.Empty,
        };

        string emailContent = EmailContentConstants.ChangeReporterIssueContent(changeReporterIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
        {
            SenderName = senderName,
            ActionName = EmailConstants.MadeOneUpdate,
            ProjectName = projectName,
            IssueCode = issue.Code,
            IssueName = issue.Name,
            EmailContent = emailContent,
        };

        await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: updateIssueDto.ModificationUserId, buidEmailTemplateBaseDto);

        issueDetail.ReporterId = reporterId;
    }
    #endregion

    public async Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto, Guid? sprintId = null, Guid? backlogId = null)
    {
        var issue = createIssueDto.Adapt<Issue>();

        if (sprintId is not null)
        {
            issue.SprintId = sprintId;
        }
        else
        {
            issue.BacklogId = backlogId;
        }

        _issueRepository.Add(issue);
        await _issueRepository.UnitOfWork.SaveChangesAsync();

        var issueDetail = new IssueDetail()
        {
            ReporterId = createIssueDto.CreatorUserId,
            StoryPointEstimate = 0,
            Label = string.Empty,
            IssueId = issue.Id,
        };

        _issueDetailRepository.Add(issueDetail);
        await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

        var issueHis = new IssueHistory
        {
            Name = "created the Issue",
            CreatorUserId = createIssueDto.CreatorUserId,
            IssueId = issue.Id,
        };

        _issueHistoryRepository.Add(issueHis);
        await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();
        return await ToIssueViewModel(issue);
    }

    public async Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto)
    {
        var issue = await _issueRepository.Get(id) ?? throw new SprintNullException();
        var issueDetail = await _issueDetailRepository.GetById(id);

        await DetachUpdateField(issue, issueDetail, updateIssueDto);

        await AddWatcher(issue, updateIssueDto);

        await ChangeSprintOrBacklog(issue, updateIssueDto);

        await AddVersionOrLabel(issue, updateIssueDto);

        issue = updateIssueDto.Adapt(issue);
        _issueRepository.Update(issue);
        await _issueRepository.UnitOfWork.SaveChangesAsync();

        UpdateIssueDetail(issueDetail, updateIssueDto);
        _issueDetailRepository.Update(issueDetail);
        await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

        return await ToIssueViewModel(issue);
    }

    public async Task<IReadOnlyCollection<IssueViewModel>> GetBySprintId(Guid sprintId)
    {
        var issues = await _issueRepository.GetIssueBySprintId(sprintId);
        return await ToIssueViewModels(issues);
    }

    public async Task<IReadOnlyCollection<IssueViewModel>> GetByBacklogId(Guid backlogId)
    {
        var issues = await _issueRepository.GetIssueByBacklogId(backlogId);
        return await ToIssueViewModels(issues);
    }

    public async Task<IssueViewModel> CreateIssueByName(CreateIssueByNameDto createIssueByNameDto, Guid? sprintId = null, Guid? backlogId = null)
    {
        var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createIssueByNameDto.ProjectId);
        int issueIndex = projectConfiguration.IssueCode + 1;
        var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createIssueByNameDto.ProjectId);
        var creatorUser = await _userManager.FindByIdAsync(createIssueByNameDto.CreatorUserId.ToString());

        var issue = new Issue()
        {
            Name = createIssueByNameDto.Name,
            IssueTypeId = createIssueByNameDto.IssueTypeId,
            Code = $"{projectConfiguration.Code}-{issueIndex}",
            StatusId = createTransition.ToStatusId,
            PriorityId = projectConfiguration.DefaultPriorityId,
            Watcher = new(),
            ParentId = createIssueByNameDto.ParentId
        };

        if (sprintId is not null)
        {
            issue.SprintId = sprintId;
        }
        else
        {
            issue.BacklogId = backlogId;
        }

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

        var issueDetail = new IssueDetail()
        {
            ReporterId = createIssueByNameDto.CreatorUserId,
            StoryPointEstimate = 0,
            Label = string.Empty,
            IssueId = issue.Id,
            AssigneeId = projectConfiguration.DefaultAssigneeId,
        };

        _issueDetailRepository.Add(issueDetail);
        await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

        var issueHis = new IssueHistory
        {
            Name = "created the Issue",
            CreatorUserId = createIssueByNameDto.CreatorUserId,
            IssueId = issue.Id,
        };

        if (sprintId is Guid newSprintId)
        {
            string? newSprintName = await _sprintRepository.GetNameOfSprint(newSprintId);
            var changedTheParentHis = new IssueHistory()
            {
                Name = IssueConstants.Parent_IssueHistoryName,
                Content = $"{IssueConstants.None_IssueHistoryContent} to {newSprintName}",
                CreatorUserId = createIssueByNameDto.CreatorUserId,
                IssueId = issue.Id,
            };
            _issueHistoryRepository.Add(changedTheParentHis);
        }

        _issueHistoryRepository.Add(issueHis);
        await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

        projectConfiguration.IssueCode = issueIndex;
        _projectConfigurationRepository.Update(projectConfiguration);
        await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

        var reporterName = await _userManager.Users.Where(u => u.Id == createIssueByNameDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;

        var createdIssueEmailContentDto = new CreatedIssueEmailContentDto(reporterName, issue.CreationTime)
        {
            IssueTypeName = await _issueTypeRepository.GetNameOfIssueType(createIssueByNameDto.IssueTypeId) ?? IssueConstants.None_IssueHistoryContent,
            AssigneeName = await _userManager.Users.Where(u => u.Id == issueDetail.AssigneeId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.Unassigned_IssueHistoryContent,
            PriorityName = issue.PriorityId is not null ? await _priorityRepository.GetNameOfPriority((Guid)issue.PriorityId) ?? IssueConstants.None_IssueHistoryContent : IssueConstants.None_IssueHistoryContent,
        };

        string emailContent = EmailContentConstants.CreatedIssueContent(createdIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
        {
            SenderName = await _userManager.Users.Where(u => u.Id == createIssueByNameDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent,
            ActionName = EmailConstants.CreatedAnIssue,
            ProjectName = await _projectRepository.GetProjectName(createIssueByNameDto.ProjectId),
            IssueCode = issue.Code,
            IssueName = issue.Name,
            EmailContent = emailContent,
        };

        await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: createIssueByNameDto.CreatorUserId, buidEmailTemplateBaseDto);
        return await ToIssueViewModel(issue);
    }

    public async Task<Guid> DeleteIssue(Guid id)
    {
        _issueRepository.Delete(id);
        await _issueRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<ChildIssueViewModel> CreateChildIssue(CreateChildIssueDto createChildIssueDto)
    {
        var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createChildIssueDto.ProjectId);
        int issueIndex = projectConfiguration.IssueCode + 1;
        var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createChildIssueDto.ProjectId);
        var creatorUser = await _userManager.FindByIdAsync(createChildIssueDto.CreatorUserId.ToString());

        var issueType = await _issueTypeRepository.GetSubtask();

        var issue = new Issue()
        {
            Name = createChildIssueDto.Name,
            IssueTypeId = issueType.Id,
            Code = $"{projectConfiguration.Code}-{issueIndex}",
            ParentId = createChildIssueDto.ParentId,
            PriorityId = projectConfiguration.DefaultPriorityId,
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
        var issueVM = _mapper.Map<ChildIssueViewModel>(issue);

        var issueDetail = new IssueDetail()
        {
            ReporterId = createChildIssueDto.CreatorUserId,
            StoryPointEstimate = 0,
            Label = string.Empty,
            IssueId = issue.Id,
            AssigneeId = projectConfiguration.DefaultAssigneeId,
        };

        _issueDetailRepository.Add(issueDetail);
        await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

        var issueHis = new IssueHistory
        {
            Name = "created the Issue",
            CreatorUserId = createChildIssueDto.CreatorUserId,
            IssueId = issue.Id,
        };

        _issueHistoryRepository.Add(issueHis);
        await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

        projectConfiguration.IssueCode = issueIndex;
        _projectConfigurationRepository.Update(projectConfiguration);
        await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

        return issueVM;
    }

    public async Task<IssueViewModel> GetById(Guid id)
    {
        var issue = await _issueRepository.Get(id);
        return await ToIssueViewModel(issue!);
    }

    public async Task<IReadOnlyCollection<IssueHistoryViewModel>> GetHistoriesByIssueId(Guid issueId)
    {
        var issueHistories = await _issueHistoryRepository.GetByIssueId(issueId);
        foreach (var issueHistory in issueHistories)
        {
            if (issueHistory.Name is IssueConstants.Reporter_IssueHistoryName)
            {
                var reporterFromTo = issueHistory.Content.FromJson<ReporterFromTo>();
                var fromName = await _userManager.Users.Where(u => u.Id == reporterFromTo.From).Select(u => u.Name).FirstOrDefaultAsync();
                var toName = await _userManager.Users.Where(u => u.Id == reporterFromTo.To).Select(u => u.Name).FirstOrDefaultAsync();
                issueHistory.Content = $"{fromName} to {toName}";
            }
            else if (issueHistory.Name is IssueConstants.Assignee_IssueHistoryName)
            {
                var assigneeFromTo = issueHistory.Content.FromJson<AssigneeFromTo>();
                var fromName = assigneeFromTo.From is not null ? await _userManager.Users.Where(u => u.Id == assigneeFromTo.From).Select(u => u.Name).FirstOrDefaultAsync() : IssueConstants.Unassigned_IssueHistoryContent;
                var toName = assigneeFromTo.To is not null ? await _userManager.Users.Where(u => u.Id == assigneeFromTo.To).Select(u => u.Name).FirstOrDefaultAsync() : IssueConstants.Unassigned_IssueHistoryContent;
                issueHistory.Content = $"{fromName} to {toName}";
            }
        }
        return _mapper.Map<IReadOnlyCollection<IssueHistoryViewModel>>(issueHistories);
    }

    public async Task<IReadOnlyCollection<IssueForProjectViewModel>> GetIssuesForProject(Guid projectId)
    {
        var backlog = await _backlogRepository.GetByProjectId(projectId) ?? throw new BacklogNullException();
        var issuesOfBacklog = await _backlogRepository.GetIssues(backlog.Id);
        var sprints = await _sprintRepository.GetSprintByProjectId(projectId);
        var issues = new List<Issue>();
        issues.AddRange(issuesOfBacklog);

        if (sprints.Any())
        {
            foreach (var sprint in sprints)
            {
                var issuesOfSprint = await _sprintRepository.GetIssues(sprint.Id, projectId);
                issues.AddRange(issuesOfSprint);
            }
        }

        var epics = await _issueRepository.GetEpicByProjectId(projectId);
        issues.AddRange(epics);

        return await ToIssueForProjectViewModels(issues);
    }
}
