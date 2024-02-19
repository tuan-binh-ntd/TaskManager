namespace TaskManager.Application.PermissionGroups.Commands.Delete;

public sealed class DeletePermissionGroupCommand(
    Guid permissionGroupId,
    Guid? newPermissionGroupId
    )
    : ICommand<Result<Guid>>
{
    public Guid PermissionGroupId { get; private set; } = permissionGroupId;
    public Guid? NewPermissionGroupId { get; private set; } = newPermissionGroupId;
}
