namespace TaskManager.Core.Interfaces.Repositories;

public interface IAttachmentRepository
{
    Task<IReadOnlyCollection<AttachmentViewModel>> GetByIssueId(Guid issueId);
    void InsertRange(IReadOnlyCollection<Attachment> attachments);
    void Insert(Attachment attachment);
    Task<Attachment?> GetByIdAsync(Guid id);
    void Remove(Attachment attachment);
}
