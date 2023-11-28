using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsFavourite { get; set; } = false;

        //Relationship
        public Backlog? Backlog { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; }
        public ICollection<IssueType>? IssueTypes { get; set; }
        public ProjectConfiguration? ProjectConfiguration { get; set; }
        public ICollection<Status>? Statuses { get; set; }
        public ICollection<Transition>? Transitions { get; set; }
        public Workflow? Workflow { get; set; }
        public ICollection<Priority>? Priorities { get; set; }
        public ICollection<PermissionGroup>? PermissionGroups { get; set; }
        public ICollection<Sprint>? Sprints { get; set; }
        public ICollection<Version>? Versions { get; set; }
    }
}
