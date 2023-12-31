﻿namespace TaskManager.Core.ViewModel;

public class UserViewModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string? Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public IReadOnlyCollection<Guid> PermissionGroups { get; set; } = new List<Guid>();
}

public class MemberProjectViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid PermissionGroupId { get; set; }
    public string Email { get; set; } = string.Empty;
}
