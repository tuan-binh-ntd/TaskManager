using Azure;
using Azure.Storage.Files.Shares;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly FileShareSettings _fileShareSettings;
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(
            IAttachmentRepository attachmentRepository,
            IOptionsMonitor<FileShareSettings> optionsMonitor
            )
        {
            _fileShareSettings = optionsMonitor.CurrentValue;
            _attachmentRepository = attachmentRepository;
        }

        #region Private method
        private async Task<string> FileUploadAsync(IFormFile file)
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
            ShareFileClient shareFile = directory.GetFileClient(file.FileName);

            using Stream stream = file.OpenReadStream();
            shareFile.Create(stream.Length);

            int blockSize = 8000 * 4000;
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

        private async Task<Response> DeleteFileAsync(string fileName)
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
            return reponse;
        }
        #endregion

        public async Task<AttachmentViewModel> Create(Guid issueId, IFormFile file)
        {
            var uploadfileUri = await FileUploadAsync(file);
            if (string.IsNullOrWhiteSpace(uploadfileUri))
            {
                return new AttachmentViewModel();
            }
            else
            {
                var attachment = new Attachment()
                {
                    Name = file.FileName,
                    Link = uploadfileUri,
                    IssueId = issueId,
                };

                _attachmentRepository.Add(attachment);
                await _attachmentRepository.UnitOfWork.SaveChangesAsync();
                return attachment.Adapt<AttachmentViewModel>();
            }
        }

        public async Task<Guid> Delete(Guid id)
        {
            var attachment = await _attachmentRepository.GetById(id) ?? throw new AttachmentNullException();
            var deleteFileResponse = await DeleteFileAsync(attachment.Name);
            if (!deleteFileResponse.IsError)
            {

                _attachmentRepository.Delete(attachment);
                await _attachmentRepository.UnitOfWork.SaveChangesAsync();
                return id;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
