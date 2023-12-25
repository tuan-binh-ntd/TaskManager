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
        versionViewModel.Issues = await ToIssueViewModels(issues);
        return versionViewModel;
    }

    private async Task<IReadOnlyCollection<VersionViewModel>> ToVersionViewModels(IReadOnlyCollection<Version> versions)
    {
        var versionViewModels = new List<VersionViewModel>();
        if (versions.Count > 0)
        {
            foreach (var version in versions)
            {
                var versionViewModel = await ToVersionViewModel(version);
                versionViewModels.Add(versionViewModel);
            }
        }
        return versionViewModels.AsReadOnly();
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
        await _issueRepository.LoadIssueDetail(issue);
        await _issueRepository.LoadStatus(issue);
        await _issueRepository.LoadIssueType(issue);
        var issueViewModel = _mapper.Map<IssueViewModel>(issue);
        if (issue.IssueDetail is not null)
        {
            var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
            issueViewModel.IssueDetail = issueDetail;
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
        if (issue.IssueType is not null)
        {
            var issueType = _mapper.Map<IssueTypeViewModel>(issue.IssueType);
            issueViewModel.IssueType = issueType;
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

    public async Task<Guid> Delete(Guid id, Guid? newVersionId)
    {
        var versionIssues = await _versionRepository.GetVersionIssuesByVersionId(id);
        _versionRepository.RemoveRange(versionIssues);
        if (newVersionId is Guid versionId)
        {
            var newVersionIssues = new List<VersionIssue>();
            foreach (var versionIssue in versionIssues)
            {
                var newVersionIssue = new VersionIssue()
                {
                    VersionId = versionId,
                    IssueId = versionIssue.IssueId
                };
                newVersionIssues.Add(newVersionIssue);
            }
            _versionRepository.AddRange(newVersionIssues);
        }
        _versionRepository.Delete(id);
        await _versionRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<IReadOnlyCollection<VersionViewModel>> GetByProjectId(Guid projectId)
    {
        var versions = await _versionRepository.GetByProjectId(projectId);
        return await ToVersionViewModels(versions);
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

        version.Name = string.IsNullOrWhiteSpace(updateVersionDto.Name) ? version.Name : updateVersionDto.Name;
        version.StartDate = updateVersionDto.StartDate is DateTime startDate ? startDate : version.StartDate;
        version.ReleaseDate = updateVersionDto.ReleaseDate is DateTime releaseDate ? releaseDate : version.ReleaseDate;
        version.Description = string.IsNullOrWhiteSpace(updateVersionDto.Description) ? version.Description : updateVersionDto.Description;
        version.DriverId = updateVersionDto.DriverId is Guid driverId ? driverId : version.DriverId;
        version.StatusId = updateVersionDto.StatusId is Guid statusId ? statusId : version.StatusId;

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
