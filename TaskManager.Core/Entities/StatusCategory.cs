namespace TaskManager.Core.Entities;

public class StatusCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    //Relationship
    public ICollection<Status>? Statuses { get; set; }
}
