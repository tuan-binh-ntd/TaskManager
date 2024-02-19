namespace TaskManager.Application.PermissionGroups.Commands.Update;

public sealed class UpdatePermissionGroupCommand(
    Guid permissionGroupId,
    UpdatePermissionGroupDto updatePermissionGroupDto
    )
    : ICommand<Result<PermissionGroupViewModel>>
{
    public Guid PermissionGroupId { get; private set; } = permissionGroupId;
    public UpdatePermissionGroupDto UpdatePermissionGroupDto { get; private set; } = updatePermissionGroupDto;
}
