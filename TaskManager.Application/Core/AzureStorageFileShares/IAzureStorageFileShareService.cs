namespace TaskManager.Application.Core.AzureStorageFileShares;

public interface IAzureStorageFileShareService
{
    Task<string> FileUploadAsync(IFormFile file, string fileName);
    Task<int> DeleteFileAsync(string fileName);
    Task<string> UploadPhotoForUserAsync(IFormFile file);
    Task<int> DeletePhotoForUserAsync(string fileName);
}
