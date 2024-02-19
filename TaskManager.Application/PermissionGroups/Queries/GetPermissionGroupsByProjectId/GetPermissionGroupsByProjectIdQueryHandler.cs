namespace TaskManager.Application.PermissionGroups.Queries.GetPermissionGroupsByProjectId;

internal sealed class GetPermissionGroupsByProjectIdQueryHandler(
    IPermissionGroupRepository permissionGroupRepository
    )
    : IQueryHandler<GetPermissionGroupsByProjectIdQuery, Maybe<object>>
{
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;

    public async Task<Maybe<object>> Handle(GetPermissionGroupsByProjectIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PaginationInput.IsPaging())
        {
            var permissionGroups = await _permissionGroupRepository.GetPermissionGroupViewModelsByProjectIdPagingAsync(request.ProjectId, request.PaginationInput);
            return Result.Success(permissionGroups);
        }
        else
        {
            var permissionGroups = await _permissionGroupRepository.GetPermissionGroupViewModelsByProjectIdAsync(request.ProjectId);
            return Result.Success(permissionGroups);
        }
    }
}
