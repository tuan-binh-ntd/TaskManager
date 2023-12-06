using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.Shares;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
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
        private readonly IIssueHistoryRepository _issueHistoryRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        private readonly BlobServiceClient _blobClient;
        private readonly BlobContainerClient _containerClient;

        public AttachmentService(
            IAttachmentRepository attachmentRepository,
            IOptionsMonitor<FileShareSettings> optionsMonitor,
            IOptionsMonitor<BlobContainerSettings> optionsMonitor1,
            IIssueHistoryRepository issueHistoryRepository,
            IIssueRepository issueRepository,
            IEmailSender emailSender,
            UserManager<AppUser> userManager
            )
        {
            _fileShareSettings = optionsMonitor.CurrentValue;
            _blobContainerSettings = optionsMonitor1.CurrentValue;
            _attachmentRepository = attachmentRepository;
            _issueHistoryRepository = issueHistoryRepository;
            _issueRepository = issueRepository;
            _emailSender = emailSender;
            _userManager = userManager;
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

        public async Task<IReadOnlyCollection<AttachmentViewModel>> CreateMultiple(Guid issueId, List<IFormFile> files, Guid userId)
        {
            var attachments = new List<Attachment>();
            var issueHistories = new List<IssueHistory>();
            var issue = await _issueRepository.Get(issueId);

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

                    var issueHistory = new IssueHistory()
                    {
                        Name = IssueConstants.Added_Attachment_IssueHistoryName,
                        Content = $"{IssueConstants.None_IssueHistoryContent} to {file.FileName}",
                        CreatorUserId = userId,
                        IssueId = issueId,
                    };


                    attachments.Add(attachment);
                    issueHistories.Add(issueHistory);
                }
            }
            _attachmentRepository.AddRange(attachments);
            _issueHistoryRepository.AddRange(issueHistories);
            await _attachmentRepository.UnitOfWork.SaveChangesAsync();

            foreach (var attachment in attachments)
            {
                var senderName = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
                var projectName = await _issueRepository.GetProjectNameOfIssue(issueId);

                var addNewAttachmentIssueEmailContentDto = new AddNewAttachmentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
                {
                    AttachmentName = attachment.Name,
                };

                string emailContent = EmailContentConstants.AddNewAttachmentIssueContent(addNewAttachmentIssueEmailContentDto);

                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
                {
                    SenderName = senderName,
                    ActionName = EmailConstants.AddOneNewAttachment,
                    ProjectName = projectName,
                    IssueCode = issue.Code,
                    IssueName = issue.Name,
                    EmailContent = emailContent,
                };

                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: userId, buidEmailTemplateBaseDto);
            }

            return attachments.Adapt<IReadOnlyCollection<AttachmentViewModel>>();
        }

        public async Task<Guid> Delete(Guid id, Guid userId, Guid issueId)
        {
            var attachment = await _attachmentRepository.GetById(id) ?? throw new AttachmentNullException();
            var issue = await _issueRepository.Get(issueId);
            var issueHistory = new IssueHistory()
            {
                Name = IssueConstants.Deleted_Attachment_IssueHistoryName,
                Content = $"{attachment.Name} to {IssueConstants.None_IssueHistoryContent}",
                CreatorUserId = userId,
                IssueId = issueId,
            };

            _issueHistoryRepository.Add(issueHistory);
            _attachmentRepository.Delete(attachment);
            var rowAffected = await _attachmentRepository.UnitOfWork.SaveChangesAsync();

            var senderName = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
            var projectName = await _issueRepository.GetProjectNameOfIssue(issueId);

            var deleteNewAttachmentIssueEmailContentDto = new DeleteNewAttachmentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue)
            {
                AttachmentName = attachment.Name,
            };

            string emailContent = EmailContentConstants.DeleteNewAttachmentIssueContent(deleteNewAttachmentIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.DeleteOneNewAttachment,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: userId, buidEmailTemplateBaseDto);

            if (rowAffected > 0)
            {
                await DeleteFileAsync(attachment.Code);
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
