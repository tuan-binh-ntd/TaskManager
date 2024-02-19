namespace TaskManager.Core.Interfaces.Repositories;

public interface ILabelIssueRepository
{
    Task<IReadOnlyCollection<LabelIssue>> GetLabelIssuesByIssueIdAsync(Guid issueId);
    Task<IReadOnlyCollection<LabelIssue>> GetLabelIssuesByLabelIdAsync(Guid labelId);
    void RemoveRange(IReadOnlyCollection<LabelIssue> labelIssues);
    void InsertRange(IReadOnlyCollection<LabelIssue> labelIssues);
}
