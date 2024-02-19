namespace TaskManager.Application.PermissionGroups.Commands.Create;

internal sealed class CreatePermissionGroupCommandHandler(
    IPermissionGroupRepository permissionGroupRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreatePermissionGroupCommand, Result<PermissionGroupViewModel>>
{
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PermissionGroupViewModel>> Handle(CreatePermissionGroupCommand createPermissionGroupCommand, CancellationToken cancellationToken)
    {
        var permissions = new Permissions
        {
            Timeline = createPermissionGroupCommand.CreatePermissionGroupDto.Timeline,
            Backlog = createPermissionGroupCommand.CreatePermissionGroupDto.Backlog,
            Board = createPermissionGroupCommand.CreatePermissionGroupDto.Board,
            Project = createPermissionGroupCommand.CreatePermissionGroupDto.Project,
        };

        var permissionGroup = PermissionGroup.Create(createPermissionGroupCommand.CreatePermissionGroupDto.Name,
            permissions.ToJson(),
            false,
            createPermissionGroupCommand.CreatePermissionGroupDto.ProjectId
            );

        _permissionGroupRepository.Insert(permissionGroup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(permissionGroup.ToViewModel());
    }
}
