﻿namespace TaskManager.Core.Entities;

public class AppRole : IdentityRole<Guid>
{
    // Relationship
    public ICollection<AppUserRole>? UserRoles { get; set; }
}
