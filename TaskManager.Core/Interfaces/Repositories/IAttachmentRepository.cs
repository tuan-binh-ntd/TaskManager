using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IAttachmentRepository : IRepository<Attachment>
{
    Task<IReadOnlyCollection<AttachmentViewModel>> GetByIssueId(Guid issueId);
    AttachmentViewModel Add(Attachment attachment);
    void Update(Attachment attachment);
    void Delete(Attachment attachment);
    Task<Attachment?> GetById(Guid id);
    void AddRange(IReadOnlyCollection<Attachment> attachments);
}
