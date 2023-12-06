using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
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
            return issueViewModel;
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

        private async Task DetachUpdateField(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto)
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

            await ChangeStartDateIssue(issue, updateIssueDto, issueHistories, senderName, projectName);

            await ChangeDueDateIssue(issue, updateIssueDto, issueHistories, senderName, projectName);

            _issueHistoryRepository.AddRange(issueHistories);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task ChangeNameIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
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

        private async Task ChangeParentIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid parentId, string senderName, string projectName)
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

        private async Task ChangeAssigneeIssue(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
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

        private async Task ChangeStatusIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid newStatusId, string senderName, string projectName)
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

        private async Task ChangePriorityIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid newPriorityId, string senderName, string projectName)
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

        private async Task ChangeSPEIssue(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
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

        private async Task ChangeReporterIssue(Issue issue, IssueDetail issueDetail, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, Guid reporterId, string senderName, string projectName)
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

        private async Task ChangeStartDateIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
        {
            var updatedTheStartDateHis = new IssueHistory()
            {
                Name = IssueConstants.StartDate_EpicHistoryName,
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };

            var changeStartDateIssueEmailContentDto = new ChangeStartDateIssueEmailContentDto(senderName, issue.CreationTime);

            if (updateIssueDto.StartDate != DateTime.MinValue && updateIssueDto.StartDate is DateTime newStartDate && issue.StartDate is null)
            {
                updatedTheStartDateHis.Content = $"{IssueConstants.None_IssueHistoryContent} to {newStartDate:MMM dd, yyyy}";

                changeStartDateIssueEmailContentDto.FromStartDate = IssueConstants.None_IssueHistoryContent;
                changeStartDateIssueEmailContentDto.ToStartDate = newStartDate.ToString("dd/MMM/yy");

                issue.StartDate = newStartDate;
            }
            if (updateIssueDto.StartDate != DateTime.MinValue && updateIssueDto.StartDate is DateTime newStartDate1 && issue.StartDate is DateTime oldStartDate1)
            {
                updatedTheStartDateHis.Content = $"{oldStartDate1:MMM dd, yyyy} to {newStartDate1:MMM dd, yyyy}";

                changeStartDateIssueEmailContentDto.FromStartDate = oldStartDate1.ToString("dd/MMM/yy");
                changeStartDateIssueEmailContentDto.ToStartDate = newStartDate1.ToString("dd/MMM/yy");

                issue.StartDate = newStartDate1;
            }
            if (updateIssueDto.StartDate != DateTime.MinValue && updateIssueDto.StartDate is null && issue.StartDate is DateTime oldStartDate2)
            {
                updatedTheStartDateHis.Content = $"{oldStartDate2:MMM dd, yyyy} to {IssueConstants.None_IssueHistoryContent}";

                changeStartDateIssueEmailContentDto.FromStartDate = oldStartDate2.ToString("dd/MMM/yy");
                changeStartDateIssueEmailContentDto.ToStartDate = IssueConstants.None_IssueHistoryContent;

                issue.StartDate = null;
                updateIssueDto.StartDate = null;
            }

            if (updateIssueDto.StartDate != issue.StartDate)
            {

                string emailContent = EmailContentConstants.ChangeStartDateIssueContent(changeStartDateIssueEmailContentDto);

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

                issueHistories.Add(updatedTheStartDateHis);
            }
        }

        private async Task ChangeDueDateIssue(Issue issue, UpdateEpicDto updateIssueDto, List<IssueHistory> issueHistories, string senderName, string projectName)
        {
            var updatedTheDueDateHis = new IssueHistory()
            {
                Name = IssueConstants.DueDate_EpicHistoryName,
                CreatorUserId = updateIssueDto.ModificationUserId,
                IssueId = issue.Id,
            };

            var changeDueDateIssueEmailContentDto = new ChangeDueDateIssueEmailContentDto(senderName, issue.CreationTime);

            if (updateIssueDto.DueDate != DateTime.MinValue && updateIssueDto.DueDate is DateTime newDueDate && issue.DueDate is null)
            {
                updatedTheDueDateHis.Content = $"{IssueConstants.None_IssueHistoryContent} to {newDueDate:MMM dd, yyyy}";

                changeDueDateIssueEmailContentDto.FromDueDate = IssueConstants.None_IssueHistoryContent;
                changeDueDateIssueEmailContentDto.ToDueDate = newDueDate.ToString("dd/MMM/yy");

                issue.DueDate = newDueDate;
            }
            if (updateIssueDto.DueDate != DateTime.MinValue && updateIssueDto.DueDate is DateTime newDueDate1 && issue.DueDate is DateTime oldDueDate1)
            {
                updatedTheDueDateHis.Content = $"{oldDueDate1:MMM dd, yyyy} to {newDueDate1:MMM dd, yyyy}";

                changeDueDateIssueEmailContentDto.FromDueDate = oldDueDate1.ToString("dd/MMM/yy");
                changeDueDateIssueEmailContentDto.ToDueDate = newDueDate1.ToString("dd/MMM/yy");

                issue.DueDate = newDueDate1;

            }
            if (updateIssueDto.DueDate != DateTime.MinValue && updateIssueDto.DueDate is null && issue.DueDate is DateTime oldDueDate2)
            {
                updatedTheDueDateHis.Content = $"{oldDueDate2:MMM dd, yyyy} to {IssueConstants.None_IssueHistoryContent}";

                changeDueDateIssueEmailContentDto.FromDueDate = oldDueDate2.ToString("dd/MMM/yy");
                changeDueDateIssueEmailContentDto.ToDueDate = IssueConstants.None_IssueHistoryContent;

                issue.DueDate = null;
                updateIssueDto.DueDate = null;
            }

            if (updateIssueDto.DueDate != issue.DueDate)
            {
                string emailContent = EmailContentConstants.ChangeDueDateIssueContent(changeDueDateIssueEmailContentDto);

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

                issueHistories.Add(updatedTheDueDateHis);
            }

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

        public async Task<EpicViewModel> CreateEpic(CreateEpicDto createEpicDto)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createEpicDto.ProjectId);
            int issueIndex = projectConfiguration.IssueCode + 1;
            var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createEpicDto.ProjectId);
            var creatorUser = await _userManager.FindByIdAsync(createEpicDto.CreatorUserId.ToString());

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

            return epicViewModel;
        }

        public async Task<GetIssuesByEpicIdViewModel> GetIssuesByEpicId(Guid epicId)
        {
            var epic = await _issueRepository.Get(epicId);
            var childIssues = await _issueRepository.GetChildIssueOfEpic(epicId);
            return await ToGetIssuesByEpicIdViewModel(epic, childIssues);
        }

        public async Task<EpicViewModel> UpdateEpic(Guid id, UpdateEpicDto updateEpicDto)
        {
            var epic = await _issueRepository.Get(id);
            var issueDetail = await _issueDetailRepository.GetById(id);
            await DetachUpdateField(epic, issueDetail, updateEpicDto);
            if (epic is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(epic));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            _issueRepository.Update(epic);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            if (updateEpicDto.StoryPointEstimate is not null)
            {
                issueDetail.StoryPointEstimate = (int)updateEpicDto.StoryPointEstimate;
            }
            if (updateEpicDto.ReporterId is not null)
            {
                issueDetail.ReporterId = (Guid)updateEpicDto.ReporterId;
            }
            if (updateEpicDto.AssigneeId is not null)
            {
                issueDetail.AssigneeId = updateEpicDto.AssigneeId;
            }
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();
            return await ToEpicViewModel(epic);
        }
    }
}
