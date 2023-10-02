using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// LeaderId --> UserId
        /// </summary>
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsFavourite { get; set; } = false;

        //Relationship
        public Backlog? Backlog { get; set; }
        public Guid LeaderId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
