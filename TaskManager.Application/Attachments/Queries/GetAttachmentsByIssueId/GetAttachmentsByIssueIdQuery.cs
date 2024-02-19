namespace TaskManager.Application.Attachments.Queries.GetAttachmentsByIssueId;

public sealed class GetAttachmentsByIssueIdQuery(
    Guid issueId
    )
    : IQuery<Maybe<IReadOnlyCollection<AttachmentViewModel>>>
{
    public Guid IssueId { get; private set; } = issueId;
}
