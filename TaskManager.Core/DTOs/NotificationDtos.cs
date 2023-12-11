namespace TaskManager.Core.DTOs;

public class CreateNotificationEventDto
{
    public Guid EventId { get; set; }
    public bool AllWatcher { get; set; }
    public bool CurrentAssignee { get; set; }
    public bool Reporter { get; set; }
    public bool ProjectLead { get; set; }
}

public class UpdateNotificationEventDto
{
    public bool AllWatcher { get; set; }
    public bool CurrentAssignee { get; set; }
    public bool Reporter { get; set; }
    public bool ProjectLead { get; set; }
}