namespace TaskManager.Application.Priorities.Commands.Update;

internal sealed class UpdatePriorityCommandHandler(
    IPriorityRepository priorityRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdatePriorityCommand, Result<PriorityViewModel>>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PriorityViewModel>> Handle(UpdatePriorityCommand updatePriorityCommand, CancellationToken cancellationToken)
    {
        var priority = await _priorityRepository.GetByIdAsync(updatePriorityCommand.PriorityId);
        if (priority is null) return Result.Failure<PriorityViewModel>(Error.NotFound);
        priority = updatePriorityCommand.UpdatePriorityDto.Adapt<Priority>();
        _priorityRepository.Update(priority);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(priority.Adapt<PriorityViewModel>());
    }
}
