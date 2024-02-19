namespace TaskManager.Application.Attachments.Commands.Delete;

public sealed class DeleteAttachmentCommandHandler(
    IAttachmentRepository attachmentRepository,
    IAzureStorageFileShareService azureStorageFileShareService,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteAttachmentCommand, Result<Guid>>
{
    private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;
    private readonly IAzureStorageFileShareService _azureStorageFileShareService = azureStorageFileShareService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _attachmentRepository.GetByIdAsync(request.AttachmentId);
        if (attachment is null) return Result.Failure<Guid>(Error.NotFound);

        attachment.AttachmentDeleted(request.UserId);
        _attachmentRepository.Remove(attachment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _azureStorageFileShareService.DeleteFileAsync(attachment.Code);
        return Result.Success(request.AttachmentId);
    }
}
