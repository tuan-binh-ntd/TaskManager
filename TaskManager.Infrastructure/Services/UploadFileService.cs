using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net.Sockets;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.Infrastructure.Services;

public class UploadFileService : IUploadFileService
{
    private readonly SftpServerSettings _sftpServerSettings;
    private readonly FileShareSettings _fileShareSettings;

    public UploadFileService(
        IOptionsMonitor<SftpServerSettings> sftpSettingsOption,
        IOptionsMonitor<FileShareSettings> fileShareSettings
        )
    {
        _sftpServerSettings = sftpSettingsOption.CurrentValue;
        _fileShareSettings = fileShareSettings.CurrentValue;
    }
    public bool UploadFile(IFormFile file)
    {
        using SftpClient client = new(_sftpServerSettings.Localhost, _sftpServerSettings.Port, _sftpServerSettings.UserName, _sftpServerSettings.Password);
        try
        {
            client.Connect();
            if (client.IsConnected)
            {
                string fileName = Path.GetFileName(file.FileName);
                using var stream = file.OpenReadStream();
                client.ChangeDirectory("/upload");
                client.UploadFile(stream, $"{fileName}");
                client.Disconnect();
                return true;
            }
            return false;
        }
        catch (Exception e) when (e is SshConnectionException || e is SocketException || e is ProxyException)
        {
            Console.WriteLine($"Error connecting to server: {e.Message}");
            return false;
        }
        catch (SshAuthenticationException e)
        {
            Console.WriteLine($"Failed to authenticate: {e.Message}");
            return false;
        }
        catch (SftpPermissionDeniedException e)
        {
            Console.WriteLine($"Operation denied by the server: {e.Message}");
            return false;
        }
        catch (SshException e)
        {
            Console.WriteLine($"Sftp Error: {e.Message}");
            return false;
        }
    }

    public async Task FileUploadAsync(FileDetails fileDetails)
    {
        // Get the configurations and create share object
        ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);

        // Create the share if it doesn't already exist
        await share.CreateIfNotExistsAsync();

        // Check the file share is present or not
        if (await share.ExistsAsync())
        {
            // Get a reference to the sample directory
            ShareDirectoryClient directory = share.GetDirectoryClient("FileShareDemoFiles");

            // Create the directory if it doesn't already exist
            await directory.CreateIfNotExistsAsync();

            // Ensure that the directory exists
            if (await directory.ExistsAsync())
            {
                // Get a reference to a file and upload it
                ShareFileClient file = directory.GetFileClient(fileDetails.FileDetail!.FileName);

                using Stream stream = fileDetails.FileDetail.OpenReadStream();
                file.Create(stream.Length);
                file.UploadRange(new HttpRange(0, stream.Length), stream);
            }
        }
        else
        {
            Console.WriteLine($"File is not upload successfully");
        }
    }

    public async Task FileDownloadAsync(string fileShareName)
    {
        ShareClient share = new(_fileShareSettings.ConnectionStrings, _fileShareSettings.FileShareName);
        ShareDirectoryClient directory = share.GetDirectoryClient("FileShareDemoFiles");
        ShareFileClient file = directory.GetFileClient(fileShareName);

        // Check path
        var filesPath = Directory.GetCurrentDirectory() + "/files";
        if (!Directory.Exists(filesPath))
        {
            Directory.CreateDirectory(filesPath);
        }

        var fileName = Path.GetFileName(fileShareName);
        var filePath = Path.Combine(filesPath, fileName);

        // Download the file
        ShareFileDownloadInfo download = file.Download();
        using FileStream stream = File.OpenWrite(filePath);
        await download.Content.CopyToAsync(stream);
    }
}
