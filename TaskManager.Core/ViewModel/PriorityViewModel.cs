namespace TaskManager.Core.ViewModel;

public class PriorityViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public int IssueCount { get; set; }
}
