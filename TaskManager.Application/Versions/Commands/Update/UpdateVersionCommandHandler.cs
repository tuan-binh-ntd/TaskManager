namespace TaskManager.Application.Versions.Commands.Update;

internal sealed class UpdateVersionCommandHandler(
    IVersionRepository versionRepository,
    IStatusRepository statusRepository,
    IVersionIssueRepository versionIssueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateVersionCommand, Result<VersionViewModel>>
{
    private readonly IVersionRepository _versionRepository = versionRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IVersionIssueRepository _versionIssueRepository = versionIssueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<VersionViewModel>> Handle(UpdateVersionCommand updateVersionCommand, CancellationToken cancellationToken)
    {
        var version = await _versionRepository.GetByIdAsync(updateVersionCommand.VersionId);
        if (version is null) return Result.Failure<VersionViewModel>(Error.NotFound);

        version.Name = string.IsNullOrWhiteSpace(updateVersionCommand.UpdateVersionDto.Name) ? version.Name : updateVersionCommand.UpdateVersionDto.Name;
        version.StartDate = updateVersionCommand.UpdateVersionDto.StartDate is DateTime startDate ? startDate : version.StartDate;
        version.ReleaseDate = updateVersionCommand.UpdateVersionDto.ReleaseDate is DateTime releaseDate ? releaseDate : version.ReleaseDate;
        version.Description = string.IsNullOrWhiteSpace(updateVersionCommand.UpdateVersionDto.Description) ? version.Description : updateVersionCommand.UpdateVersionDto.Description;
        version.DriverId = updateVersionCommand.UpdateVersionDto.DriverId is Guid driverId ? driverId : version.DriverId;

        if (updateVersionCommand.UpdateVersionDto.StatusId is Guid statusId)
        {
            version.StatusId = statusId;
            if (await _statusRepository.IsReleaseStatusAsync(statusId)
                && updateVersionCommand.UpdateVersionDto.NewVersionId is Guid newId)
            {
                await _versionIssueRepository.UpdateVersionIdAsync(updateVersionCommand.VersionId, newId);

            }
        }

        _versionRepository.Update(version);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(version.Adapt<VersionViewModel>());
    }
}
