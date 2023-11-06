using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
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
            _issueRepository.LoadEntitiesRelationship(issue);
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
            if (issue.ParentId is not null)
            {
                issueViewModel.ParentName = await _issueRepository.GetParentName(issue.Id);
            }
            if (childIssues.Any())
            {
                issueViewModel.ChildIssues = _mapper.Map<ICollection<ChildIssueViewModel>>(childIssues);
            }
            return issueViewModel;
        }

        private async Task<EpicViewModel> ToEpicViewModel(Issue epic)
        {
            _issueRepository.LoadEntitiesRelationship(epic);
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
                epicViewModel.ChildIssues = _mapper.Map<ICollection<IssueViewModel>>(childIssues);
            }
            return epicViewModel;
        }

        private async Task<GetIssuesByEpicIdViewModel> ToGetIssuesByEpicIdViewModel(Issue epic, IReadOnlyCollection<Issue> childIssues)
        {
            var epicViewModel = _mapper.Map<EpicViewModel>(epic);
            epicViewModel.ChildIssues = _mapper.Map<ICollection<IssueViewModel>>(childIssues);

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

            var issueType = await _issueTypeRepository.GetEpic();

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
            if (epic is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(epic));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            epic = updateEpicDto.Adapt(epic);
            _issueRepository.Update(epic);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueDetail = await _issueDetailRepository.GetById(id);
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
