namespace TaskManager.Application.PermissionGroups.Commands.Delete;

internal sealed class DeletePermissionGroupCommandHandler(
    IPermissionGroupRepository permissionGroupRepository,
    IUserProjectRepository userProjectRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeletePermissionGroupCommand, Result<Guid>>
{
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeletePermissionGroupCommand deletePermissionGroupCommand, CancellationToken cancellationToken)
    {
        var permissionGroup = await _permissionGroupRepository.GetByIdAsync(deletePermissionGroupCommand.PermissionGroupId);
        if (permissionGroup is null) return Result.Failure<Guid>(Error.NotFound);
        if (deletePermissionGroupCommand.NewPermissionGroupId is Guid newId)
        {
            await _userProjectRepository.UpdatePermissionGroupIdAsync(deletePermissionGroupCommand.PermissionGroupId, newId);
        }
        _permissionGroupRepository.Remove(permissionGroup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(permissionGroup.Id);
    }
}
