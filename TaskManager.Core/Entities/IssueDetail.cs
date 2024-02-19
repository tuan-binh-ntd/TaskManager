namespace TaskManager.Core.Entities;

public class IssueDetail : BaseEntity
{
    private IssueDetail(Guid? assigneeId, Guid reporterId, int storyPointEstimate, Guid issueId)
    {
        AssigneeId = assigneeId;
        ReporterId = reporterId;
        StoryPointEstimate = storyPointEstimate;
        IssueId = issueId;
    }

    private IssueDetail() { }

    public Guid? AssigneeId { get; set; }
    public Guid ReporterId { get; set; }
    public int StoryPointEstimate { get; set; }

    //Relationship
    public Guid IssueId { get; set; }
    public Issue? Issue { get; set; }

    public static IssueDetail Create(Guid? assigneeId, Guid reporterId, int storyPointEstimate, Guid issueId)
    {
        return new IssueDetail(assigneeId, reporterId, storyPointEstimate, issueId);
    }
}
