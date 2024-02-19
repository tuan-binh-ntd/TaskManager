using TaskManager.Core.Events.Attachments;

namespace TaskManager.Core.Entities;

public class Attachment : BaseEntity
{
    private Attachment(string name, string link, long size, string type, string code, Guid issueId)
    {
        Name = name;
        Link = link;
        Size = size;
        Type = type;
        Code = code;
        IssueId = issueId;
    }

    private Attachment() { }

    public string Name { get; private set; } = string.Empty;
    public string Link { get; private set; } = string.Empty;
    public long Size { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;

    //Relationship
    public Guid IssueId { get; private set; }
    public Issue? Issue { get; set; }

    public static Attachment Create(string name, string link, long size, string type, string code, Guid issueId)
    {
        return new Attachment(name, link, size, type, code, issueId);
    }

    public void AttachmentCreated(Guid userId)
    {
        AddDomainEvent(new AttachmentCreatedDomainEvent(Name, IssueId, userId));
    }

    public void AttachmentDeleted(Guid userId)
    {
        AddDomainEvent(new AttachmentDeletedDomainEvent(Name, IssueId, userId));
    }
}
