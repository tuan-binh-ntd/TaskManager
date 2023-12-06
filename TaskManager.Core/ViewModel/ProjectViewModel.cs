namespace TaskManager.Core.ViewModel
{
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
    }

    public class BacklogViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<IssueViewModel>? Issues { get; set; }
    }

    public class StatusViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }

    public class StatusCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
