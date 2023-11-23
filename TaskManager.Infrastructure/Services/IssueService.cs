using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
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
                    Progress = (doneChildIssuesNum / childIssuesNum) * 100,
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
                    Progress = 0,
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
            var issue = await _issueRepository.Get(id);
            if (issue is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(issue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
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

            issue = updateIssueDto.Adapt(issue);
            _issueRepository.Update(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueDetail = await _issueDetailRepository.GetById(id);
            if (updateIssueDto.StoryPointEstimate is not null)
            {
                issueDetail.StoryPointEstimate = (int)updateIssueDto.StoryPointEstimate;
            }
            if (updateIssueDto.ReporterId is not null)
            {
                issueDetail.ReporterId = (Guid)updateIssueDto.ReporterId;
            }
            if (updateIssueDto.AssigneeId is not null)
            {
                issueDetail.AssigneeId = updateIssueDto.AssigneeId;
            }

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

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

            projectConfiguration.IssueCode = issueIndex;
            _projectConfigurationRepository.Update(projectConfiguration);
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

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
                    var issuesOfSprint = await _sprintRepository.GetIssues(sprint.Id);
                    issues.AddRange(issuesOfSprint);
                }
            }

            return await ToIssueForProjectViewModels(issues);
        }
    }
}
