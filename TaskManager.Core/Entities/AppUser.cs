
using TaskManager.Core.Events.Users;

namespace TaskManager.Core.Entities;

public class AppUser
    : IdentityUser<Guid>, IHasDomainEvent
{
    private AppUser(string name, string department, string organization, string avatarUrl, string jobTitle, string location, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Department = department;
        Organization = organization;
        AvatarUrl = avatarUrl;
        JobTitle = jobTitle;
        Location = location;
        Email = email;
    }
    private AppUser()
    {

    }

    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public override string? Email { get; set; }
    // Relationship
    public ICollection<AppUserRole>? UserRoles { get; set; }
    public ICollection<UserProject>? UserProjects { get; set; }
    public ICollection<UserTeam>? UserTeams { get; set; }
    public ICollection<UserNotification>? Notifications { get; set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public static AppUser Create(string name, string department, string organization, string avatarUrl, string jobTitle, string location, string email)
    {
        return new AppUser(name, department, organization, avatarUrl, jobTitle, location, email);
    }

    public void UserRegistered() => AddDomainEvent(new UserRegisteredDomainEvent(Id));
}
