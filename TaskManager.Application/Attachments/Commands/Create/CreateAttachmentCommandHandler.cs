namespace TaskManager.Application.Attachments.Commands.Create;

public sealed class CreateAttachmentCommandHandler(
    IAttachmentRepository attachmentRepository,
    IAzureStorageFileShareService azureStorageFileShareService,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateAttachmentCommand, Result<IReadOnlyCollection<AttachmentViewModel>>>
{
    private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;
    private readonly IAzureStorageFileShareService _azureStorageFileShareService = azureStorageFileShareService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyCollection<AttachmentViewModel>>> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachments = new List<Attachment>();

        foreach (var file in request.Files)
        {
            var code = $"{Guid.NewGuid()}_{file.FileName}";
            var uploadfileUri = await _azureStorageFileShareService.FileUploadAsync(file, fileName: code);
            if (!string.IsNullOrWhiteSpace(uploadfileUri))
            {
                var attachment = Attachment.Create(file.FileName, uploadfileUri, file.Length, file.ContentType, code, request.IssueId);
                attachment.AttachmentCreated(request.UserId);
                attachments.Add(attachment);
            }
        }
        _attachmentRepository.InsertRange(attachments);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(attachments.Adapt<IReadOnlyCollection<AttachmentViewModel>>());
    }
}
