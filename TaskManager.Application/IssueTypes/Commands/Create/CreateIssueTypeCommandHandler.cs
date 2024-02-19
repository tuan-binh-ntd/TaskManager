namespace TaskManager.Application.IssueTypes.Commands.Create;

internal sealed class CreateIssueTypeCommandHandler(
    IIssueTypeRepository issueTypeRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateIssueTypeCommand, Result<IssueTypeViewModel>>
{
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IssueTypeViewModel>> Handle(CreateIssueTypeCommand createIssueTypeCommand, CancellationToken cancellationToken)
    {
        var issueType = createIssueTypeCommand.CreateIssueTypeDto.Adapt<IssueType>();
        issueType.ProjectId = createIssueTypeCommand.ProjectId;
        issueType.Level = 2;

        _issueTypeRepository.Insert(issueType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(issueType.Adapt<IssueTypeViewModel>());
    }
}
