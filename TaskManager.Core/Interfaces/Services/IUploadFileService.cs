using Microsoft.AspNetCore.Http;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Interfaces.Services;

public interface IUploadFileService
{
    bool UploadFile(IFormFile file);
    Task FileUploadAsync(FileDetails fileDetails);
    Task FileDownloadAsync(string fileShareName);
}
