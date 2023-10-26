using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class UserTeam : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid TeamId { get; set; }
        public AppUser? User { get; set; }
        public Team? Team { get; set; }
    }
}
