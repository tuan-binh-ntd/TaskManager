using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Version : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// someone in your team with project administrator permissions who’s responsible for coordinating the release from start to finish (by default, it’s the person who creates the version).
        /// </summary>
        public Guid DriverId { get; set; }
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
        public ICollection<VersionIssue>? VersionIssues { get; set; }
        public Guid StatusId { get; set; }
        public Status? Status { get; set; }
    }
}
