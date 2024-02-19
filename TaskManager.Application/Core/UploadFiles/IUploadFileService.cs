namespace TaskManager.Application.Core.UploadFiles;

public interface IUploadFileService
{
    bool UploadFile(IFormFile file);
    Task FileDownloadAsync(string fileShareName);
}
