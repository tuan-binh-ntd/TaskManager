namespace TaskManager.Application.IssueTypes.Commands.Update;

internal sealed class UpdateIssueTypeCommandHandler(
    IIssueTypeRepository issueTypeRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    )
    : ICommandHandler<UpdateIssueTypeCommand, Result<IssueTypeViewModel>>
{
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IssueTypeViewModel>> Handle(UpdateIssueTypeCommand updateIssueTypeCommand, CancellationToken cancellationToken)
    {
        var issuesType = await _issueTypeRepository.GetByIdAsync(updateIssueTypeCommand.IssueTypeId);

        if (issuesType is null) return Result.Failure<IssueTypeViewModel>(Error.NotFound);

        issuesType = _mapper.Map<IssueType>(updateIssueTypeCommand.UpdateIssueTypeDto);
        _issueTypeRepository.Update(issuesType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(issuesType.Adapt<IssueTypeViewModel>());
    }
}
