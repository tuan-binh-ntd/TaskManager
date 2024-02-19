namespace TaskManager.Application.Statuses.Commands.Create;

internal sealed class CreateStatusCommandHandler(
    IStatusRepository statusRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateStatusCommand, Result<StatusViewModel>>
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<StatusViewModel>> Handle(CreateStatusCommand createStatusCommand, CancellationToken cancellationToken)
    {
        var status = createStatusCommand.CreateStatusDto.Adapt<Status>();
        _statusRepository.Insert(status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(status.Adapt<StatusViewModel>());
    }
}
