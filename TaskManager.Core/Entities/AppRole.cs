using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        //public Guid? ParentId { get; set; }

        // Relationship
        public ICollection<AppUserRole>? UserRoles { get; set; }
        //public ICollection<PermissionRole>? PermissionRoles { get; set; }
    }
}
