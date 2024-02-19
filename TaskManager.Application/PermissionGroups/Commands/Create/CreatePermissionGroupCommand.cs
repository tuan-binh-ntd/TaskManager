namespace TaskManager.Application.PermissionGroups.Commands.Create;

public sealed class CreatePermissionGroupCommand(
    CreatePermissionGroupDto createPermissionGroupDto
    )
    : ICommand<Result<PermissionGroupViewModel>>
{
    public CreatePermissionGroupDto CreatePermissionGroupDto { get; private set; } = createPermissionGroupDto;
}
