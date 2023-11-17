using Mapster;
using MapsterMapper;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectConfigurationRepository _projectConfigurationRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly ITransitionRepository _transitionRepository;
        private readonly IBacklogRepository _backlogRepository;
        private readonly IMapper _mapper;

        public SprintService(
            ISprintRepository sprintRepository,
            IProjectConfigurationRepository projectConfigurationRepository,
            IIssueRepository issueRepository,
            ITransitionRepository transitionRepository,
            IBacklogRepository backlogRepository,
            IMapper mapper
            )
        {
            _sprintRepository = sprintRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
            _issueRepository = issueRepository;
            _transitionRepository = transitionRepository;
            _backlogRepository = backlogRepository;
            _mapper = mapper;
        }

        #region Private method
        private async Task<SprintViewModel> ToSprintViewModel(Sprint sprint)
        {
            var issues = await _issueRepository.GetIssueBySprintId(sprint.Id);
            var issueViewModels = await ToIssueViewModels(issues);
            var sprintViewModel = _mapper.Map<SprintViewModel>(sprint);
            sprintViewModel.Issues = issueViewModels;
            return sprintViewModel;
        }
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
        #endregion

        public async Task<SprintViewModel> CreateNoFieldSprint(Guid projectId)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(projectId);
            int sprintIndex = projectConfiguration.SprintCode + 1;

            Sprint sprint = new()
            {
                Name = $"{projectConfiguration.Code} Sprint {sprintIndex}",
                StartDate = null,
                EndDate = null,
                Goal = string.Empty,
                ProjectId = projectId,
                IsStart = false,
                IsComplete = false,
            };
            _sprintRepository.Add(sprint);
            var result = await _sprintRepository.UnitOfWork.SaveChangesAsync();

            if (result > 0)
            {
                projectConfiguration.SprintCode = sprintIndex;
                _projectConfigurationRepository.Update(projectConfiguration);
                await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();
            }
            return sprint.Adapt<SprintViewModel>();
        }

        public async Task<SprintViewModel> CreateSprint(CreateSprintDto createSprintDto)
        {
            var sprint = createSprintDto.Adapt<Sprint>();
            var sprintVM = _sprintRepository.Add(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprintVM;
        }

        public async Task<Guid> DeleteSprint(Guid id)
        {
            var issues = await _sprintRepository.GetIssues(id);
            _issueRepository.DeleteRange(issues);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            _sprintRepository.Delete(id);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<SprintViewModel> StartSprint(Guid projectId, Guid sprintId, UpdateSprintDto updateSprintDto)
        {
            var sprint = _sprintRepository.Get(sprintId);
            sprint = updateSprintDto.Adapt(sprint);
            sprint!.IsStart = true;
            _sprintRepository.Update(sprint!);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();

            var issues = await _sprintRepository.GetIssues(sprintId);
            var createTransition = _transitionRepository.GetCreateTransitionByProjectId(projectId);
            foreach (var issue in issues)
            {
                issue.StatusId = createTransition.ToStatusId;
            }
            _issueRepository.UpdateRange(issues);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var sprintViewModel = _mapper.Map<SprintViewModel>(sprint!);
            sprintViewModel.Issues = _mapper.Map<IReadOnlyCollection<IssueViewModel>>(issues);
            return sprintViewModel;
        }

        public async Task<SprintViewModel> UpdateSprint(Guid id, UpdateSprintDto updateSprintDto)
        {
            var sprint = _sprintRepository.Get(id) ?? throw new SprintNullException();
            sprint = updateSprintDto.Adapt(sprint);
            _sprintRepository.Update(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprint.Adapt<SprintViewModel>();
        }

        public async Task<SprintViewModel> CompleteSprint(Guid sprintId, Guid projectId, CompleteSprintDto completeSprintDto)
        {
            var sprint = _sprintRepository.Get(sprintId) ?? throw new SprintNullException();

            var issues = await _issueRepository.GetNotDoneIssuesBySprintId(sprintId, projectId);

            if (completeSprintDto.Option == CoreConstants.NewSprintOption)
            {
                var newSprint = await CreateNoFieldSprint(projectId);

                foreach (var issue in issues)
                {
                    issue.SprintId = newSprint.Id;
                }
                _issueRepository.UpdateRange(issues);
                await _issueRepository.UnitOfWork.SaveChangesAsync();
            }
            else if (completeSprintDto.Option == CoreConstants.BacklogOption)
            {
                var backlog = await _backlogRepository.GetByProjectId(projectId) ?? throw new BacklogNullException();
                foreach (var issue in issues)
                {
                    issue.SprintId = null;
                    issue.BacklogId = backlog.Id;
                }
                _issueRepository.UpdateRange(issues);
                await _issueRepository.UnitOfWork.SaveChangesAsync();
            }
            else
            {
                if (completeSprintDto.SprintId is Guid specificSprintId)
                {
                    var specificSprint = _sprintRepository.Get(specificSprintId) ?? throw new SprintNullException();
                    foreach (var issue in issues)
                    {
                        issue.SprintId = specificSprint.Id;
                    }
                    _issueRepository.UpdateRange(issues);
                    await _issueRepository.UnitOfWork.SaveChangesAsync();
                }
            }

            sprint.IsComplete = true;
            sprint.IsStart = false;
            _sprintRepository.Update(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprint.Adapt<SprintViewModel>();
        }

        public async Task<SprintViewModel> GetById(Guid sprintId)
        {
            var sprint = _sprintRepository.Get(sprintId) ?? throw new SprintNullException();
            return await ToSprintViewModel(sprint);
        }
    }
}
