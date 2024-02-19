namespace TaskManager.Infrastructure.Services;

public class AzureStorageFileShareService(
    IOptionsMonitor<FileShareSettings> optionsMonitor
    )
    : IAzureStorageFileShareService
{
    private readonly FileShareSettings _fileShareSettings = optionsMonitor.CurrentValue;

    public async Task<int> DeleteFileAsync(string fileName)
    {
        ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);

        // Create the share if it doesn't already exist
        await share.CreateIfNotExistsAsync();

        // Get a reference to the sample directory
        ShareDirectoryClient directory = share.GetDirectoryClient("AttachmentOfIssue");

        // Create the directory if it doesn't already exist
        await directory.CreateIfNotExistsAsync();

        // Get a reference to a file and upload it
        ShareFileClient shareFile = directory.GetFileClient(fileName);
        var reponse = shareFile.Delete();
        return reponse.Status;
    }

    public async Task<string> FileUploadAsync(IFormFile file, string fileName)
    {
        // Get the configurations and create share object
        ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);

        // Create the share if it doesn't already exist
        await share.CreateIfNotExistsAsync();

        // Get a reference to the sample directory
        ShareDirectoryClient directory = share.GetDirectoryClient("AttachmentOfIssue");

        // Create the directory if it doesn't already exist
        await directory.CreateIfNotExistsAsync();

        // Get a reference to a file and upload it
        ShareFileClient shareFile = directory.GetFileClient(fileName);

        using Stream stream = file.OpenReadStream();
        shareFile.Create(stream.Length);

        int blockSize = 4194304;
        long offset = 0;//Define http range offset
        BinaryReader reader = new(stream);
        while (true)
        {
            byte[] buffer = reader.ReadBytes(blockSize);
            if (offset == stream.Length)
            {
                break;
            }
            else
            {
                MemoryStream uploadChunk = new();
                uploadChunk.Write(buffer, 0, buffer.Length);
                uploadChunk.Position = 0;

                HttpRange httpRange = new(offset, buffer.Length);
                var response = shareFile.UploadRange(httpRange, uploadChunk);
                offset += buffer.Length;//Shift the offset by number of bytes already written
            }
        }
        reader.Close();

        return shareFile.Uri.AbsoluteUri;
    }

    public async Task<string> UploadPhotoForUserAsync(IFormFile file)
    {
        // Get the configurations and create share object
        ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);

        // Create the share if it doesn't already exist
        await share.CreateIfNotExistsAsync();

        // Get a reference to the sample directory
        ShareDirectoryClient directory = share.GetDirectoryClient("AvatarOfUser");

        // Create the directory if it doesn't already exist
        await directory.CreateIfNotExistsAsync();

        // Get a reference to a file and upload it
        ShareFileClient shareFile = directory.GetFileClient(file.FileName);

        using Stream stream = file.OpenReadStream();
        shareFile.Create(stream.Length);

        int blockSize = 4194304;
        long offset = 0;//Define http range offset
        BinaryReader reader = new(stream);
        while (true)
        {
            byte[] buffer = reader.ReadBytes(blockSize);
            if (offset == stream.Length)
            {
                break;
            }
            else
            {
                MemoryStream uploadChunk = new();
                uploadChunk.Write(buffer, 0, buffer.Length);
                uploadChunk.Position = 0;

                HttpRange httpRange = new(offset, buffer.Length);
                var response = shareFile.UploadRange(httpRange, uploadChunk);
                offset += buffer.Length;//Shift the offset by number of bytes already written
            }
        }
        reader.Close();

        return shareFile.Uri.AbsoluteUri;
    }

    public async Task<int> DeletePhotoForUserAsync(string fileName)
    {
        ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);

        // Create the share if it doesn't already exist
        await share.CreateIfNotExistsAsync();

        // Get a reference to the sample directory
        ShareDirectoryClient directory = share.GetDirectoryClient("AvatarOfUser");

        // Create the directory if it doesn't already exist
        await directory.CreateIfNotExistsAsync();

        // Get a reference to a file and upload it
        ShareFileClient shareFile = directory.GetFileClient(fileName);
        var reponse = shareFile.Delete();
        return reponse.Status;
    }
}
