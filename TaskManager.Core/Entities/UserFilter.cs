using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class UserFilter : BaseEntity
{
    public string Type { get; set; } = string.Empty;

    //Relationship
    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }
    public Guid FilterId { get; set; }
    public Filter? Filter { get; set; }
}
