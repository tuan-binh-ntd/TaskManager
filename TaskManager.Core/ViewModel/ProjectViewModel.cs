using Mapster;

namespace TaskManager.Core.ViewModel;

public class ProjectViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public bool IsFavourite { get; set; } = false;
    public UserViewModel? Leader { get; set; }
    public IReadOnlyCollection<UserViewModel>? Members { get; set; }
    public BacklogViewModel? Backlog { get; set; }
    public IReadOnlyCollection<SprintViewModel>? Sprints { get; set; }
    public IReadOnlyCollection<IssueTypeViewModel>? IssueTypes { get; set; }
    public IReadOnlyCollection<StatusViewModel>? Statuses { get; set; }
    public IReadOnlyCollection<EpicViewModel>? Epics { get; set; }
    public IReadOnlyCollection<PriorityViewModel>? Priorities { get; set; }
    public IReadOnlyCollection<StatusCategoryViewModel>? StatusCategories { get; set; }
    public PermissionGroupViewModel? UserPermissionGroup { get; set; }
    [AdaptIgnore]
    public IReadOnlyCollection<PermissionGroupViewModel> PermissionGroups { get; set; } = new List<PermissionGroupViewModel>();
    public ProjectConfigurationViewModel? ProjectConfiguration { get; set; }
    public IReadOnlyCollection<LabelViewModel>? Labels { get; set; }
}

public class BacklogViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<IssueViewModel>? Issues { get; set; }
}

public class StatusViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public Guid StatusCategoryId { get; set; }
    public int IssueCount { get; set; }
}

public class StatusCategoryViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
public class ProjectConfigurationViewModel
{
    public Guid? DefaultAssigneeId { get; set; }
    public Guid? DefaultPriorityId { get; set; }
}

public class GetIssueForProjectViewModel
{
    public BacklogViewModel Backlog { get; set; } = new();
    public IReadOnlyCollection<SprintViewModel> Sprints { get; set; } = new List<SprintViewModel>();
}
