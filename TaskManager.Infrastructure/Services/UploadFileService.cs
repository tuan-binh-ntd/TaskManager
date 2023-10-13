using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net.Sockets;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.Infrastructure.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly SftpServerSettings _sftpServerSettings;
        public UploadFileService(IOptionsMonitor<SftpServerSettings> sftpSettingsOption)
        {
            _sftpServerSettings = sftpSettingsOption.CurrentValue;
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
    }
}
