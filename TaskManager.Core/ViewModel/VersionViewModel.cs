namespace TaskManager.Core.ViewModel
{
    public class VersionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// someone in your team with project administrator permissions who’s responsible for coordinating the release from start to finish (by default, it’s the person who creates the version).
        /// </summary>
        public Guid DriverId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid StatusId { get; set; }
        public IReadOnlyCollection<IssueViewModel>? Issues { get; set; }
    }

    public class GetIssuesByVersionIdViewModel
    {
        public IReadOnlyCollection<SprintViewModel>? Sprints { get; set; }
        public BacklogViewModel? Backlog { get; set; }
        public VersionViewModel? Version { get; set; }
    }
}
