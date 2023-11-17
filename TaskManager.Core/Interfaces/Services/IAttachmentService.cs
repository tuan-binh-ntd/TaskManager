using Microsoft.AspNetCore.Http;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IAttachmentService
    {
        Task<AttachmentViewModel> Create(Guid issueId, IFormFile file);
        Task<Guid> Delete(Guid id);
    }
}
