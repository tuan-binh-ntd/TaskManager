﻿using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        // Relationship
        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    }
}
