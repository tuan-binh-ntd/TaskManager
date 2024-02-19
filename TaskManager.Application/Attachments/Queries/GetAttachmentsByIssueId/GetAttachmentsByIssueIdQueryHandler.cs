namespace TaskManager.Application.Attachments.Queries.GetAttachmentsByIssueId;

internal sealed class GetAttachmentsByIssueIdQueryHandler(
    IAttachmentRepository attachmentRepository
    )
    : IQueryHandler<GetAttachmentsByIssueIdQuery, Maybe<IReadOnlyCollection<AttachmentViewModel>>>
{
    private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;

    public async Task<Maybe<IReadOnlyCollection<AttachmentViewModel>>> Handle(GetAttachmentsByIssueIdQuery request, CancellationToken cancellationToken)
    {
        if (request.IssueId == Guid.Empty)
        {
            return Maybe<IReadOnlyCollection<AttachmentViewModel>>.None;
        }
        IReadOnlyCollection<AttachmentViewModel> response = await _attachmentRepository.GetByIssueId(request.IssueId);

        return Maybe<IReadOnlyCollection<AttachmentViewModel>>.From(response);
    }
}
