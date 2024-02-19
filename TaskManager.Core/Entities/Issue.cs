namespace TaskManager.Core.Entities;

public class Issue : BaseEntity
{
    private Issue(
        Guid id,
        string name,
        string code,
        Guid issueTypeId,
        Guid projectId,
        Watcher watcher
        )
    {
        Id = id;
        Name = name;
        Code = code;
        IssueTypeId = issueTypeId;
        ProjectId = projectId;
        Watcher = watcher;
    }

    private Issue(
        Guid id,
        string name,
        string code,
        Guid issueTypeId,
        Watcher watcher,
        Guid? priorityId
        )
    {
        Id = id;
        Name = name;
        Code = code;
        IssueTypeId = issueTypeId;
        Watcher = watcher;
        PriorityId = priorityId;
    }

    private Issue()
    {
    }

    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime? CompleteDate { get; set; }
    public Watcher? Watcher { get; set; }
    public string? Voted { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }

    //Relationship
    public Guid? ParentId { get; set; }
    public Guid? SprintId { get; set; }
    public Sprint? Sprint { get; set; }
    public Guid IssueTypeId { get; set; }
    public IssueType? IssueType { get; set; }
    public Guid? BacklogId { get; set; }
    public Backlog? Backlog { get; set; }
    public ICollection<IssueHistory>? IssueHistories { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Attachment>? Attachments { get; set; }
    public IssueDetail? IssueDetail { get; set; }
    public Guid? StatusId { get; set; }
    public Status? Status { get; set; }
    public Guid? PriorityId { get; set; }
    public Priority? Priority { get; set; }
    public ICollection<VersionIssue>? VersionIssues { get; set; }
    public ICollection<LabelIssue>? LabelIssues { get; set; }
    public Guid? ProjectId { get; set; }

    #region Epic Behaviors
    public static Issue CreateEpic(string name,
        string code,
        Guid issueTypeId,
        Guid projectId,
        Watcher watcher)
    {
        return new Issue(Guid.NewGuid(), name, code, issueTypeId, projectId, watcher);
    }

    public void EpicCreated(CreateEpicDto createEpicDto)
        => AddDomainEvent(new EpicCreatedDomainEvent(createEpicDto, this, IssueDetail!));

    public void EpicDeleted(Guid userId)
        => AddDomainEvent(new EpicDeletedDomainEvent(this, userId));

    public void EpicUpdatedAssignee(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedAssgineeDomainEvent(this, updateEpicDto));

    public void EpicUpdatedDescription(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedDescriptionDomainEvent(this, updateEpicDto));

    public void EpicUpdatedDueDate(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedDueDateDomainEvent(this, updateEpicDto));

    public void EpicUpdatedLabel(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedLabelDomainEvent(this, updateEpicDto));

    public void EpicUpdatedName(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedNameDomainEvent(this, updateEpicDto));

    public void EpicUpdatedParent(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedParentDomainEvent(this, updateEpicDto));

    public void EpicUpdatedPriority(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedPriorityDomainEvent(this, updateEpicDto));

    public void EpicUpdatedReporter(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedReporterDomainEvent(this, updateEpicDto));

    public void EpicUpdatedSPE(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedSPEDomainEvent(this, updateEpicDto));


    public void EpicUpdatedStartDate(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedStartDateDomainEvent(this, updateEpicDto));

    public void EpicUpdatedStatus(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedStatusDomainEvent(this, updateEpicDto));

    public void EpicUpdatedVersion(UpdateEpicDto updateEpicDto)
        => AddDomainEvent(new EpicUpdatedVersionDomainEvent(this, updateEpicDto));
    #endregion

    #region Issue Behaviors
    public static Issue CreateIssue(string name,
        string code,
        Guid issueTypeId,
        Watcher watcher,
        Guid? priorityId)
    {
        return new Issue(Guid.NewGuid(), name, code, issueTypeId, watcher, priorityId);
    }

    public void IssueUpdatedAssignee(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedAssigneeDomainEvent(this, updateIssueDto));

    public void IssueUpdatedBacklog(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedBacklogDomainEvent(this, updateIssueDto));

    public void IssueUpdatedDescription(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedDescriptionDomainEvent(this, updateIssueDto));

    public void IssueUpdatedIssueType(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedIssueTypeDomainEvent(this, updateIssueDto));

    public void IssueUpdatedLabel(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedLabelDomainEvent(this, updateIssueDto));

    public void IssueUpdatedName(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedNameDomainEvent(this, updateIssueDto));

    public void IssueUpdatedParent(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedParentDomainEvent(this, updateIssueDto));

    public void IssueUpdatedPriority(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedPriorityDomainEvent(this, updateIssueDto));

    public void IssueUpdatedReporter(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedReporterDomainEvent(this, updateIssueDto));

    public void IssueUpdatedSPE(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedSPEDomainEvent(this, updateIssueDto));

    public void IssueUpdatedSprint(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedSprintDomainEvent(this, updateIssueDto));

    public void IssueUpdatedStatus(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedStatusDomainEvent(this, updateIssueDto));

    public void IssueUpdatedVersion(UpdateIssueDto updateIssueDto)
        => AddDomainEvent(new IssueUpdatedVersionDomainEvent(this, updateIssueDto));
    #endregion
}

public class Watcher
{
    public List<User>? Users { get; set; } = [];

    private Watcher(User user)
    {
        Users.Add(user);
    }

    private Watcher() { }

    public static Watcher CreateFirstWatcher(User user)
    {
        return new Watcher(user);
    }
}

public class User
{
    private User(Guid identity)
    {
        Identity = identity;
    }

    private User() { }

    public Guid Identity { get; set; }

    public static User Create(Guid identity)
        => new(identity);
}

public class AssigneeFromTo
{
    public Guid? From { get; set; }
    public Guid? To { get; set; }
}

public class ReporterFromTo
{
    public Guid From { get; set; }
    public Guid To { get; set; }
}
