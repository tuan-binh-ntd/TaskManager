namespace TaskManager.Application.Statuses.Commands.Update;

internal sealed class UpdateStatusCommandHandler(
    IStatusRepository statusRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateStatusCommand, Result<StatusViewModel>>
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<StatusViewModel>> Handle(UpdateStatusCommand updateStatusCommand, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetByIdAsync(updateStatusCommand.StatusId);
        if (status is null) return Result.Failure<StatusViewModel>(Error.NotFound);
        status = updateStatusCommand.UpdateStatusDto.Adapt(status);
        _statusRepository.Update(status);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(status.Adapt<StatusViewModel>());
    }
}
