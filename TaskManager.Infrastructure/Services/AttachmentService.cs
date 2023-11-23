using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.Shares;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly FileShareSettings _fileShareSettings;
        private readonly BlobContainerSettings _blobContainerSettings;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly BlobServiceClient _blobClient;
        private readonly BlobContainerClient _containerClient;

        public AttachmentService(
            IAttachmentRepository attachmentRepository,
            IOptionsMonitor<FileShareSettings> optionsMonitor,
            IOptionsMonitor<BlobContainerSettings> optionsMonitor1
            )
        {
            _fileShareSettings = optionsMonitor.CurrentValue;
            _blobContainerSettings = optionsMonitor1.CurrentValue;
            _attachmentRepository = attachmentRepository;

            _blobClient = new BlobServiceClient(_blobContainerSettings.ConnectionStrings);
            _containerClient = _blobClient.GetBlobContainerClient("attachmentofissue");
        }

        #region Private method
        private async Task<string> FileUploadAsync(IFormFile file, string fileName)
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

        private async Task FileUploadToContainerAsync(IFormFile file)
        {
            string fileName = file.FileName;
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var client = await _containerClient.UploadBlobAsync(fileName, memoryStream, default);
        }
        #endregion

        public async Task<IReadOnlyCollection<AttachmentViewModel>> CreateMultiple(Guid issueId, List<IFormFile> files)
        {
            var attachments = new List<Attachment>();
            foreach (var file in files)
            {
                var code = $"{Guid.NewGuid()}_{file.FileName}";
                var uploadfileUri = await FileUploadAsync(file, fileName: code);
                if (!string.IsNullOrWhiteSpace(uploadfileUri))
                {
                    var attachment = new Attachment()
                    {
                        Name = file.FileName,
                        Link = uploadfileUri,
                        Size = file.Length,
                        Type = file.ContentType,
                        IssueId = issueId,
                        Code = code
                    };

                    attachments.Add(attachment);
                }
            }
            _attachmentRepository.AddRange(attachments);
            await _attachmentRepository.UnitOfWork.SaveChangesAsync();
            return attachments.Adapt<IReadOnlyCollection<AttachmentViewModel>>();
        }

        public async Task<Guid> Delete(Guid id)
        {
            var attachment = await _attachmentRepository.GetById(id) ?? throw new AttachmentNullException();
            var deleteFileResponse = await DeleteFileAsync(attachment.Code);
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

        public async Task<IReadOnlyCollection<AttachmentViewModel>> UploadFiles(Guid issueId, List<IFormFile> files)
        {
            var attachmentViewModels = new List<AttachmentViewModel>();
            foreach (var file in files)
            {
                await FileUploadToContainerAsync(file);
                var attachment = new Attachment
                {
                    Name = file.FileName,
                    Link = string.Empty,
                    Size = file.Length,
                    Type = file.ContentType,
                    IssueId = issueId,
                };
                _attachmentRepository.Add(attachment);
                await _attachmentRepository.UnitOfWork.SaveChangesAsync();
                attachmentViewModels.Add(attachment.Adapt<AttachmentViewModel>());
            };

            return attachmentViewModels;
        }

        public async Task<string> GetUploadedBlobs()
        {
            var items = new List<BlobItem>();
            var uploadedFiles = _containerClient.GetBlobsAsync();
            await foreach (BlobItem file in uploadedFiles)
            {
                items.Add(file);
            }

            return items.ToJson();
        }

        public async Task<IReadOnlyCollection<AttachmentViewModel>> GetByIssueId(Guid issueId)
        {
            return await _attachmentRepository.GetByIssueId(issueId);
        }
    }
}
