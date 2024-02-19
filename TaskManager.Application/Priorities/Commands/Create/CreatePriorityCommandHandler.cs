namespace TaskManager.Application.Priorities.Commands.Create;

internal sealed class CreatePriorityCommandHandler(
    IPriorityRepository priorityRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreatePriorityCommand, Result<PriorityViewModel>>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PriorityViewModel>> Handle(CreatePriorityCommand createPriorityCommand, CancellationToken cancellationToken)
    {
        var priority = createPriorityCommand.CreatePriorityDto.Adapt<Priority>();
        _priorityRepository.Insert(priority);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(priority.Adapt<PriorityViewModel>());
    }
}
