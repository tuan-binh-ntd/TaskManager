using Microsoft.AspNetCore.Http;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IAttachmentService
    {
        Task<IReadOnlyCollection<AttachmentViewModel>> CreateMultiple(Guid issueId, List<IFormFile> files, Guid userId);
        Task<Guid> Delete(Guid id, Guid userId, Guid issueId);
        Task<IReadOnlyCollection<AttachmentViewModel>> UploadFiles(Guid issueId, List<IFormFile> files);
        Task<string> GetUploadedBlobs();
        Task<IReadOnlyCollection<AttachmentViewModel>> GetByIssueId(Guid issueId);
    }
}
