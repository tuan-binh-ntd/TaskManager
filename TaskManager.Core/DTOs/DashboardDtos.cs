namespace TaskManager.Core.DTOs;

public class GetIssueOfAssigneeDashboardDto
{

}

public class GetIssuesInProjectDashboardDto
{

}

public class GetIssuesForAssigneeOrReporterDto
{
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Type { get; set; } = string.Empty;
}