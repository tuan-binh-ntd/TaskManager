namespace TaskManager.Application.Versions.Commands.Create;

internal class CreateVersionCommandHandler(
    IStatusRepository statusRepository,
    IVersionRepository versionRepository,
    IUnitOfWork unitOfWork
    )
     : ICommandHandler<CreateVersionCommand, Result<VersionViewModel>>
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IVersionRepository _versionRepository = versionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<VersionViewModel>> Handle(CreateVersionCommand createVersionCommand, CancellationToken cancellationToken)
    {
        var unreleasedStatus = await _statusRepository.GetUnreleasedStatusByProjectIdAsync(createVersionCommand.CreateVersionDto.ProjectId);
        var version = createVersionCommand.CreateVersionDto.Adapt<Version>();
        version.StatusId = unreleasedStatus.Id;
        _versionRepository.Insert(version);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(version.Adapt<VersionViewModel>());
    }
}
