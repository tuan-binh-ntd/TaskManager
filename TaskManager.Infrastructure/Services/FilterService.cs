using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class FilterService : IFilterService
{
    private readonly IIssueRepository _issueRepository;
    private readonly IConnectionFactory _connectionFactory;
    private readonly IFilterRepository _filterRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IBacklogRepository _backlogRepository;
    private readonly ILogger<FilterService> _logger;
    private readonly IMapper _mapper;

    public FilterService(
        IIssueRepository issueRepository,
        IConnectionFactory connectionFactory,
        IFilterRepository filterRepository,
        ISprintRepository sprintRepository,
        IBacklogRepository backlogRepository,
        ILogger<FilterService> logger,
        IMapper mapper
        )
    {
        _issueRepository = issueRepository;
        _connectionFactory = connectionFactory;
        _filterRepository = filterRepository;
        _sprintRepository = sprintRepository;
        _backlogRepository = backlogRepository;
        _logger = logger;
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

    public async Task<FilterViewModel> CreateFilter(CreateFilterDto createFilterDto)
    {
        if (createFilterDto.Project is not null && createFilterDto.Project.ProjectIds is not null && createFilterDto.Project.ProjectIds.Any())
        {
            createFilterDto.Project.BacklogIds = await _backlogRepository.GetBacklogIdsByProjectIds(createFilterDto.Project.ProjectIds);
            createFilterDto.Project.SprintIds = await _sprintRepository.GetSprintIdsByProjectIds(createFilterDto.Project.ProjectIds);
        }
        var filterConfiguration = new FilterConfiguration()
        {
            Project = createFilterDto.Project,
            Type = createFilterDto.Type,
            Status = createFilterDto.Status,
            Assginee = createFilterDto.Assginee,
            Created = createFilterDto.Created,
            DueDate = createFilterDto.DueDate,
            FixVersions = createFilterDto.FixVersions,
            Labels = createFilterDto.Labels,
            Priority = createFilterDto.Priority,
            Reporter = createFilterDto.Reporter,
            Resolution = createFilterDto.Resolution,
            Resolved = createFilterDto.Resolved,
            Sprints = createFilterDto.Sprints,
            StatusCategory = createFilterDto.StatusCategory,
            Updated = createFilterDto.Updated,
        };

        var filter = new Filter()
        {
            Name = createFilterDto.Name,
            Stared = createFilterDto.Stared,
            Type = createFilterDto.Stared ? CoreConstants.StaredFiltersType : CoreConstants.CreatedUserFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = createFilterDto.CreatorUserId
        };

        _filterRepository.Add(filter);
        await _filterRepository.UnitOfWork.SaveChangesAsync();

        return new FilterViewModel()
        {
            Id = filter.Id,
            Name = filter.Name,
            Stared = filter.Stared,
            Type = filter.Type,
            Configuration = filter.Configuration.FromJson<FilterConfiguration>()
        };
    }

    public async Task<Guid> DeleteFilter(Guid id)
    {
        var filter = await _filterRepository.GetById(id) ?? throw new FilterNullException();
        _filterRepository.Delete(filter);
        await _filterRepository.UnitOfWork.SaveChangesAsync();
        return filter.Id;
    }

    public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByFilterConfiguration(Guid id)
    {
        var filterConfiguration = await _filterRepository.GetConfigurationOfFilter(id);
        if (filterConfiguration is not null)
        {
            string query = filterConfiguration.QueryAfterBuild();
            _logger.LogInformation(query);
            var issueIds = await _connectionFactory.QueryAsync<Guid>(query);
            if (issueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(issueIds.ToList());
                return await ToIssueViewModels(issues);
            }
        }
        return new List<IssueViewModel>();
    }

    public async Task<IReadOnlyCollection<IssueViewModel>> GetIssuesByConfiguration(GetIssueByConfigurationDto getIssueByConfigurationDto)
    {
        FilterConfiguration filterConfiguration = new()
        {
            Project = getIssueByConfigurationDto.Project,
            Type = getIssueByConfigurationDto.Type,
            Status = getIssueByConfigurationDto.Status,
            Assginee = getIssueByConfigurationDto.Assginee,
            Created = getIssueByConfigurationDto.Created,
            DueDate = getIssueByConfigurationDto.DueDate,
            FixVersions = getIssueByConfigurationDto.FixVersions,
            Labels = getIssueByConfigurationDto.Labels,
            Priority = getIssueByConfigurationDto.Priority,
            Reporter = getIssueByConfigurationDto.Reporter,
            Resolution = getIssueByConfigurationDto.Resolution,
            Resolved = getIssueByConfigurationDto.Resolved,
            Sprints = getIssueByConfigurationDto.Sprints,
            StatusCategory = getIssueByConfigurationDto.StatusCategory,
            Updated = getIssueByConfigurationDto.Updated,
        };
        if (getIssueByConfigurationDto.Project is not null && getIssueByConfigurationDto.Project.ProjectIds is not null && getIssueByConfigurationDto.Project.ProjectIds.Any())
        {
            filterConfiguration.Project!.BacklogIds = await _backlogRepository.GetBacklogIdsByProjectIds(getIssueByConfigurationDto.Project.ProjectIds);
            filterConfiguration.Project.SprintIds = await _sprintRepository.GetSprintIdsByProjectIds(getIssueByConfigurationDto.Project.ProjectIds);
        }
        string query = filterConfiguration.QueryAfterBuild();
        _logger.LogInformation(query);
        var issueIds = await _connectionFactory.QueryAsync<Guid>(query);
        if (issueIds.Any())
        {
            var issues = await _issueRepository.GetByIds(issueIds.ToList());
            return await ToIssueViewModels(issues);
        }
        return new List<IssueViewModel>();
    }

    public async Task<IReadOnlyCollection<FilterViewModel>> GetFilterViewModelsByUserId(Guid userId)
    {
        var filterViewModels = await _filterRepository.GetFiltersByUserId(userId);
        return filterViewModels;
    }

    public async Task<FilterViewModel> UpdateFilter(Guid id, UpdateFilterDto updateFilterDto)
    {
        var filter = await _filterRepository.GetById(id) ?? throw new FilterNullException();
        filter.Name = string.IsNullOrWhiteSpace(updateFilterDto.Name) ? filter.Name : updateFilterDto.Name;
        if (updateFilterDto.Stared is bool stared)
        {
            filter.Stared = stared;
        }
        filter.Type = filter.Stared ? CoreConstants.StaredFiltersType : CoreConstants.CreatedUserFiltersType;

        _filterRepository.Update(filter);
        await _filterRepository.UnitOfWork.SaveChangesAsync();
        return filter.Adapt<FilterViewModel>();
    }
}
