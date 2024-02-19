namespace TaskManager.Application.Versions.Commands.Delete;

internal sealed class DeleteVersionCommandHandler(
    IVersionRepository versionRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteVersionCommand, Result<Guid>>
{
    private readonly IVersionRepository _versionRepository = versionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteVersionCommand deleteVersionCommand, CancellationToken cancellationToken)
    {
        var version = await _versionRepository.GetByIdAsync(deleteVersionCommand.VersionId);
        if (version is null) return Result.Failure<Guid>(Error.NotFound);
        _versionRepository.Remove(version);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(deleteVersionCommand.VersionId);
    }
}
