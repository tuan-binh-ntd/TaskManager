namespace TaskManager.Core.Entities;

public class AppUserRole : IdentityUserRole<Guid>
{
    public AppUser? User { get; set; }
    public AppRole? Role { get; set; }
}
