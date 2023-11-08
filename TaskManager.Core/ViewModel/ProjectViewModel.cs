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
        public ICollection<UserViewModel>? Members { get; set; }
        public BacklogViewModel? Backlog { get; set; }
        public ICollection<SprintViewModel>? Sprints { get; set; }
        public ICollection<IssueTypeViewModel>? IssueTypes { get; set; }
        public ICollection<StatusViewModel>? Statuses { get; set; }
        public ICollection<EpicViewModel>? Epics { get; set; }
        public ICollection<PriorityViewModel>? Priorities { get; set; }
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
    }
}
