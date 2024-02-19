namespace TaskManager.Application.PermissionGroups.Commands.Update;

internal sealed class UpdatePermissionGroupCommandHandler(
    IPermissionGroupRepository permissionGroupRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdatePermissionGroupCommand, Result<PermissionGroupViewModel>>
{
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PermissionGroupViewModel>> Handle(UpdatePermissionGroupCommand updatePermissionGroupCommand, CancellationToken cancellationToken)
    {
        var permissionGroup = await _permissionGroupRepository.GetByIdAsync(updatePermissionGroupCommand.PermissionGroupId);
        if (permissionGroup is null) return Result.Failure<PermissionGroupViewModel>(Error.NotFound);
        var permissions = new Permissions
        {
            Timeline = updatePermissionGroupCommand.UpdatePermissionGroupDto.Timeline,
            Backlog = updatePermissionGroupCommand.UpdatePermissionGroupDto.Backlog,
            Board = updatePermissionGroupCommand.UpdatePermissionGroupDto.Board,
            Project = updatePermissionGroupCommand.UpdatePermissionGroupDto.Project,
        };
        permissionGroup.Name = string.IsNullOrWhiteSpace(updatePermissionGroupCommand.UpdatePermissionGroupDto.Name)
            ? permissionGroup.Name
            : updatePermissionGroupCommand.UpdatePermissionGroupDto.Name;

        permissionGroup.Permissions = permissions.ToJson();

        _permissionGroupRepository.Update(permissionGroup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(permissionGroup.ToViewModel());
    }
}
