namespace TaskManager.Core.Entities;

public class PermissionGroup : BaseEntity
{
    private PermissionGroup(string name, string permissions, bool isMain, Guid projectId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Permissions = permissions;
        IsMain = isMain;
        ProjectId = projectId;
    }

    private PermissionGroup()
    {

    }

    public string Name { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    // Relationship
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    public static PermissionGroup CreateProjectLeadRole(Guid projectId)
    {
        var plPermissions = new Permissions()
        {
            Timeline = PermissionGroupDto.Create(true, true),
            Backlog = PermissionGroupDto.Create(true, true),
            Board = PermissionGroupDto.Create(true, true),
            Project = PermissionGroupDto.Create(true, true),
        };

        return new PermissionGroup(PermissionGroupConstants.ProjectLeadName, plPermissions.ToJson(), true, projectId);
    }

    public static PermissionGroup CreateProductOwnerRole(Guid projectId)
    {
        var poPermissions = new Permissions()
        {
            Timeline = PermissionGroupDto.Create(true, true),
            Backlog = PermissionGroupDto.Create(true, true),
            Board = PermissionGroupDto.Create(true, true),
            Project = PermissionGroupDto.Create(true, true),
        };

        return new PermissionGroup(PermissionGroupConstants.ProductOwnerName, poPermissions.ToJson(), true, projectId);
    }

    public static PermissionGroup CreateScrumMasterRole(Guid projectId)
    {
        var smPermissions = new Permissions()
        {
            Timeline = PermissionGroupDto.Create(true, true),
            Backlog = PermissionGroupDto.Create(true, true),
            Board = PermissionGroupDto.Create(true, true),
            Project = PermissionGroupDto.Create(true, false),
        };

        return new PermissionGroup(PermissionGroupConstants.ScrumMasterName, smPermissions.ToJson(), true, projectId);
    }

    public static PermissionGroup CreateDeveloperRole(Guid projectId)
    {
        var devPermissions = new Permissions()
        {
            Timeline = PermissionGroupDto.Create(false, false),
            Backlog = PermissionGroupDto.Create(false, false),
            Board = PermissionGroupDto.Create(true, true),
            Project = PermissionGroupDto.Create(false, false),
        };

        return new PermissionGroup(PermissionGroupConstants.DeveloperName, devPermissions.ToJson(), true, projectId);
    }

    public static PermissionGroup Create(string name, string permissions, bool isMain, Guid projectId)
    {
        return new PermissionGroup(name, permissions, isMain, projectId);
    }
}

public class Permissions
{
    public PermissionGroupDto? Timeline { get; set; }
    public PermissionGroupDto? Backlog { get; set; }
    public PermissionGroupDto? Board { get; set; }
    public PermissionGroupDto? Project { get; set; }
}


public class PermissionGroupDto
{
    private PermissionGroupDto(bool viewPermission, bool editPermission)
    {
        ViewPermission = viewPermission;
        EditPermission = editPermission;
    }

    private PermissionGroupDto() { }

    public bool ViewPermission { get; set; }
    public bool EditPermission { get; set; }

    public static PermissionGroupDto Create(bool viewPermission, bool editPermission)
    {
        return new PermissionGroupDto(viewPermission, editPermission);
    }
}