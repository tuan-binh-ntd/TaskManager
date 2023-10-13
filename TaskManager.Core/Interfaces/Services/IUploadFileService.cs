using Microsoft.AspNetCore.Http;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IUploadFileService
    {
        bool UploadFile(IFormFile file);
    }
}
