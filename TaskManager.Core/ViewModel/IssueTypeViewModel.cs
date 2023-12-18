namespace TaskManager.Core.ViewModel;

public class IssueTypeViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Icon { get; set; } = string.Empty;
    public byte Level { get; set; }
    public bool IsMain { get; set; }
}
