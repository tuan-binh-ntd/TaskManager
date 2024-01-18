namespace TaskManager.Infrastructure.Services;

public class AttachmentService : IAttachmentService
{
    private readonly FileShareSettings _fileShareSettings;
    private readonly BlobContainerSettings _blobContainerSettings;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotificationRepository _notificationRepository;
    private readonly BlobServiceClient _blobClient;
    private readonly BlobContainerClient _containerClient;

    public AttachmentService(
        IAttachmentRepository attachmentRepository,
        IOptionsMonitor<FileShareSettings> optionsMonitor,
        IOptionsMonitor<BlobContainerSettings> optionsMonitor1,
        IIssueHistoryRepository issueHistoryRepository,
        IIssueRepository issueRepository,
        IEmailSender emailSender,
        UserManager<AppUser> userManager,
        INotificationRepository notificationRepository
        )
    {
        _fileShareSettings = optionsMonitor.CurrentValue;
        _blobContainerSettings = optionsMonitor1.CurrentValue;
        _attachmentRepository = attachmentRepository;
        _issueHistoryRepository = issueHistoryRepository;
        _issueRepository = issueRepository;
        _emailSender = emailSender;
        _userManager = userManager;
        _notificationRepository = notificationRepository;
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
        var projectId = await _issueRepository.GetProjectIdOfIssue(issue.Id);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var someoneAddedAttachmentEvent = notificationConfig.Where(n => n.EventName == CoreConstants.SomeoneMadeAAttachmentName).FirstOrDefault();

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
            var projectCode = await _issueRepository.GetProjectCodeOfIssue(issueId);

            var avatarUrl = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;


            var addNewAttachmentIssueEmailContentDto = new AddNewAttachmentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
            {
                AttachmentName = attachment.Name,
            };

            string emailContent = EmailContentConstants.AddNewAttachmentIssueContent(addNewAttachmentIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.AddOneNewAttachment, projectName, issue.Code, issue.Name, emailContent, projectCode, issueId);

            if (someoneAddedAttachmentEvent is not null)
            {
                await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: userId, buidEmailTemplateBaseDto, someoneAddedAttachmentEvent);
            }
        }

        return attachments.Adapt<IReadOnlyCollection<AttachmentViewModel>>();
    }

    public async Task<Guid> Delete(Guid id, Guid userId, Guid issueId)
    {
        var attachment = await _attachmentRepository.GetById(id) ?? throw new AttachmentNullException();
        var issue = await _issueRepository.Get(issueId);
        var projectId = await _issueRepository.GetProjectIdOfIssue(issue.Id);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var attachmentDeletedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.AttachmentDeletedName).FirstOrDefault();

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
        var projectCode = await _issueRepository.GetProjectCodeOfIssue(issueId);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var deleteNewAttachmentIssueEmailContentDto = new DeleteNewAttachmentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, avatarUrl)
        {
            AttachmentName = attachment.Name,
        };

        string emailContent = EmailContentConstants.DeleteNewAttachmentIssueContent(deleteNewAttachmentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.AddOneNewAttachment, projectName, issue.Code, issue.Name, emailContent, projectCode, issueId);

        if (attachmentDeletedEvent is not null)
        {
            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: userId, buidEmailTemplateBaseDto, attachmentDeletedEvent);
        }

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
