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


public class UserNotificationViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CreatorUsername { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public Guid IssueId { get; set; }
    public string IssueName { get; set; } = string.Empty;
    public string IssueCode { get; set; } = string.Empty;
    public IssueTypeViewModel IssueType { get; set; } = new();
    public string StatusName { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreationTime { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
}

public class RealtimeNotificationViewModel
{
    public IReadOnlyCollection<Guid> UserIds { get; set; } = new List<Guid>();
    public IssueViewModel? Issue { get; set; }
    public EpicViewModel? Epic { get; set; }
    public UserNotificationViewModel Notification { get; set; } = new();
    public Guid IssueId { get; set; }
}