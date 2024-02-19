using TaskManager.Core.Events.Projects;

namespace TaskManager.Core.Entities;

public class Project : BaseEntity
{
    private Project(Guid id, string name, string? description, string code, string avatarUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        Code = code;
        AvatarUrl = avatarUrl;
    }

    private Project() { }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

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
    public ICollection<Label>? Labels { get; set; }
    public Notification? Notification { get; set; }

    public static Project Create(string name, string? description, string code, string avatarUrl)
    {
        return new Project(Guid.NewGuid(), name, description, code, avatarUrl);
    }

    public void ProjectCreated()
    {
        AddDomainEvent(new ProjectCreatedDomainEvent(this));
    }
}
