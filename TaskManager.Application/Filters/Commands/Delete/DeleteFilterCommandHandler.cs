namespace TaskManager.Application.Filters.Commands.Delete;

internal sealed class DeleteFilterCommandHandler(
    IFilterRepository filterRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteFilterCommand, Result<Guid>>
{
    private readonly IFilterRepository _filterRepository = filterRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteFilterCommand deleteFilterCommand, CancellationToken cancellationToken)
    {
        var filter = await _filterRepository.GetByIdAsync(deleteFilterCommand.FilterId);

        if (filter is null) return Result.Failure<Guid>(Error.NotFound);

        _filterRepository.Remove(filter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(filter.Id);
    }
}
