﻿namespace TaskManager.Core.ViewModel;

public class NotificationEventViewModel
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public bool AllWatcher { get; set; }
    public bool CurrentAssignee { get; set; }
    public bool Reporter { get; set; }
    public bool ProjectLead { get; set; }
}

public class NotificationViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IReadOnlyCollection<NotificationEventViewModel> NotificationEvent { get; set; } = new List<NotificationEventViewModel>();
}
