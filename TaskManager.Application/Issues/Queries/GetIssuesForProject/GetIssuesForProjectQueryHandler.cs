namespace TaskManager.Application.Issues.Queries.GetIssuesForProject;

internal sealed class GetIssuesForProjectQueryHandler(
    IBacklogRepository backlogRepository,
    ISprintRepository sprintRepository,
    IIssueRepository issueRepository,
    IStatusCategoryRepository statusCategoryRepository
    )
    : IQueryHandler<GetIssuesForProjectQuery, Maybe<IReadOnlyCollection<IssueForProjectViewModel>>>
{
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository = statusCategoryRepository;

    #region Private methods
    private async Task<IssueForProjectViewModel> ToIssueForProjectViewModel(Issue issue)
    {
        var childIssues = await _issueRepository.GetChildIssuesOfIssueByIssueIdAsync(issue.Id);
        var doneStatusCategory = await _statusCategoryRepository.GetDoneStatusCategoryAsync() ?? throw new StatusCategoryNullException();
        if (childIssues.Any())
        {
            foreach (var childIssue in childIssues)
            {
                await _issueRepository.LoadStatusAsync(childIssue);
            }

            var epicStartDate = childIssues.MinBy(i => i.CreationTime)?.CreationTime;
            var epicEndDate = childIssues.MaxBy(i => i.CreationTime)?.CompleteDate ?? DateTime.Now;

            var doneChildIssuesNum = childIssues.Where(ci => ci.Status!.StatusCategoryId == doneStatusCategory.Id).Count();
            var childIssuesNum = childIssues.Count;

            return new IssueForProjectViewModel()
            {
                Id = issue.Id,
                Name = issue.Name,
                Start = issue.ProjectId is null ? issue.CreationTime : issue.StartDate ?? epicStartDate,
                End = issue.ProjectId is null ? issue.CompleteDate : issue.DueDate ?? epicEndDate,
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
                Start = issue.ProjectId is null ? issue.CreationTime : issue.StartDate,
                End = issue.ProjectId is null ? issue.CompleteDate : issue.DueDate,
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
    #endregion

    public async Task<Maybe<IReadOnlyCollection<IssueForProjectViewModel>>> Handle(GetIssuesForProjectQuery request, CancellationToken cancellationToken)
    {
        var backlog = await _backlogRepository.GetBacklogByProjectIdAsync(request.ProjectId) ?? throw new BacklogNullException();
        var issuesOfBacklog = await _backlogRepository.GetIssuesByBacklogIdAsync(backlog.Id);
        var sprints = await _sprintRepository.GetSprintsByProjectIdAsync(request.ProjectId);
        var issues = new List<Issue>();
        issues.AddRange(issuesOfBacklog);

        if (sprints.Any())
        {
            foreach (var sprint in sprints)
            {
                var issuesOfSprint = await _sprintRepository.GetIssuesBySprintIdAsync(sprint.Id, request.ProjectId);
                issues.AddRange(issuesOfSprint);
            }
        }

        var epics = await _issueRepository.GetEpicsByProjectIdAsync(request.ProjectId);
        issues.AddRange(epics);

        return Maybe<IReadOnlyCollection<IssueForProjectViewModel>>.From(await ToIssueForProjectViewModels(issues));
    }
}
