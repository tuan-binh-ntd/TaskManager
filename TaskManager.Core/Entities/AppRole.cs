using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole>? UserRoles { get; set; }
    }
}
