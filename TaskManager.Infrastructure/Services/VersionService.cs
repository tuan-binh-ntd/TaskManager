using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;
using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Infrastructure.Services;

public class VersionService : IVersionService
{
    private readonly IVersionRepository _versionRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IBacklogRepository _backlogRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IMapper _mapper;

    public VersionService(
        IMapper mapper,
        IVersionRepository versionRepository,
        IStatusRepository statusRepository,
        IIssueRepository issueRepository,
        IBacklogRepository backlogRepository,
        ISprintRepository sprintRepository)
    {
        _mapper = mapper;
        _versionRepository = versionRepository;
        _statusRepository = statusRepository;
        _issueRepository = issueRepository;
        _backlogRepository = backlogRepository;
        _sprintRepository = sprintRepository;
    }

    #region PrivateMethod
    private async Task<VersionViewModel> ToVersionViewModel(Version version)
    {
        var versionViewModel = _mapper.Map<VersionViewModel>(version);
        var issueIds = await _versionRepository.GetIssueIdsByVersionId(version.Id);
        var issues = await _issueRepository.GetByIds(issueIds);
        versionViewModel.Issues = issues.Adapt<IReadOnlyCollection<IssueViewModel>>();
        return versionViewModel;
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

    private async Task<GetIssuesByVersionIdViewModel> ToGetIssuesByVersionIdViewModel(Version version, IReadOnlyCollection<Issue> childIssues)
    {
        var versionViewModel = _mapper.Map<VersionViewModel>(version);
        versionViewModel.Issues = _mapper.Map<IReadOnlyCollection<IssueViewModel>>(childIssues);

        if (version.ProjectId is Guid projectId)
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

            return new GetIssuesByVersionIdViewModel()
            {
                Sprints = sprints,
                Backlog = backlog,
                Version = versionViewModel
            };
        }
        else
        {
            return new GetIssuesByVersionIdViewModel()
            {
                Sprints = new List<SprintViewModel>(),
                Backlog = new BacklogViewModel(),
                Version = versionViewModel
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
    #endregion

    public async Task<VersionViewModel> AddIssues(AddIssuesToVersionDto addIssuesToVersionDto)
    {
        var version = await _versionRepository.GetById(addIssuesToVersionDto.VersionId);
        var versionIssues = new List<VersionIssue>();
        if (addIssuesToVersionDto.IssueIds.Any())
        {
            foreach (var issueId in addIssuesToVersionDto.IssueIds)
            {

                var versionIssue = new VersionIssue()
                {
                    IssueId = issueId,
                    VersionId = addIssuesToVersionDto.VersionId
                };
                versionIssues.Add(versionIssue);
            }
        }
        _versionRepository.AddRange(versionIssues);
        await _versionRepository.UnitOfWork.SaveChangesAsync();
        return await ToVersionViewModel(version);
    }

    public async Task<VersionViewModel> Create(CreateVersionDto createVersionDto)
    {
        var unreleasedStatus = await _statusRepository.GetUnreleasedStatus(createVersionDto.ProjectId);
        var version = _mapper.Map<Version>(createVersionDto);
        version.StatusId = unreleasedStatus.Id;
        _versionRepository.Add(version);
        await _versionRepository.UnitOfWork.SaveChangesAsync();
        return _mapper.Map<VersionViewModel>(version);
    }

    public async Task<Guid> Delete(Guid id)
    {
        _versionRepository.Delete(id);
        await _versionRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<IReadOnlyCollection<VersionViewModel>> GetByProjectId(Guid projectId)
    {
        var versions = await _versionRepository.GetByProjectId(projectId);
        return _mapper.Map<IReadOnlyCollection<VersionViewModel>>(versions);
    }

    public async Task<VersionViewModel> Update(Guid id, UpdateVersionDto updateVersionDto)
    {
        var version = await _versionRepository.GetById(id);
        if (version is null)
        {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            throw new ArgumentNullException(nameof(version));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
        }

        version = _mapper.Map<Version>(updateVersionDto);
        _versionRepository.Update(version);
        await _versionRepository.UnitOfWork.SaveChangesAsync();
        return _mapper.Map<VersionViewModel>(version);
    }

    public async Task<GetIssuesByVersionIdViewModel> GetIssuesByVersionId(Guid versionId)
    {
        var version = await _versionRepository.GetById(versionId);
        var childIssues = await _issueRepository.GetChildIssueOfVersion(versionId);
        return await ToGetIssuesByVersionIdViewModel(version, childIssues);
    }
}
