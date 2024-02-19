namespace TaskManager.Application.Mapping;

public static class PermissionGroupMapping
{
    public static PermissionGroupViewModel ToViewModel(this PermissionGroup permissionGroup)
    {
        var permissionGroupViewModel = new PermissionGroupViewModel()
        {
            Id = permissionGroup.Id,
            Name = permissionGroup.Name,
            Permissions = permissionGroup.Permissions.FromJson<Permissions>()
        };

        return permissionGroupViewModel;
    }
}
