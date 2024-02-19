namespace TaskManager.Application.Statuses.Commands.Delete;

internal sealed class DeleteStatusCommandHandler(
    IStatusRepository statusRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteStatusCommand, Result<Guid>>
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteStatusCommand deleteStatusCommand, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetByIdAsync(deleteStatusCommand.StatusId);
        if (status is null) return Result.Failure<Guid>(Error.NotFound);
        _statusRepository.Remove(status);
        status.StatusDeleted(deleteStatusCommand.NewStatusId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(deleteStatusCommand.StatusId);
    }
}
