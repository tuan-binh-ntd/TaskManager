namespace TaskManager.Core.ViewModel;

public class IssueOfAssigneeDashboardViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IssueCount { get; set; }
}

public class IssuesInProjectDashboardViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IssueCount { get; set; }
}
