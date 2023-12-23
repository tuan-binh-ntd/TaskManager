namespace TaskManager.Core.DTOs;

public class CreateVersionDto
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
}

public class UpdateVersionDto
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Description { get; set; }
    /// <summary>
    /// someone in your team with project administrator permissions who’s responsible for coordinating the release from start to finish (by default, it’s the person who creates the version).
    /// </summary>
    public Guid? DriverId { get; set; }
    public Guid StatusId { get; set; }
}

public class AddIssuesToVersionDto
{
    public Guid VersionId { get; set; }
    public IEnumerable<Guid> IssueIds { get; set; } = new List<Guid>();
}
