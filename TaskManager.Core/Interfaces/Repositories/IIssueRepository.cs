namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueRepository
{
    Task<Issue?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all issue for sprint
    /// </summary>
    /// <param name="sprintId">Id of sprint entity</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetIssuesBySprintIdAsync(Guid sprintId);
    /// <summary>
    /// Get all issue for backlog
    /// </summary>
    /// <param name="backlogId">Id of backlog entity</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetIssuesByBacklogIdAsync(Guid backlogId);
    /// <summary>
    /// Get all child of issue
    /// </summary>
    /// <param name="parentId">Id of parent issue entity</param>
    /// <returns>List of child issue</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssuesByParentIdAsync(Guid parentId);
    /// <summary>
    /// Load full related entities to issue
    /// Use Explicit Loading
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadEntitiesRelationshipAsync(Issue issue);
    /// <summary>
    /// Get issues by id list
    /// </summary>
    /// <param name="ids">List of id</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetIssuesByIdsAsync(IReadOnlyCollection<Guid> ids);
    /// <summary>
    /// Get epic by projectId
    /// </summary>
    /// <param name="projectId">Id of project</param>
    /// <returns>List of epic</returns>
    Task<IReadOnlyCollection<Issue>> GetEpicsByProjectIdAsync(Guid projectId);
    /// <summary>
    /// Get child issue of epic
    /// </summary>
    /// <param name="epicId">Id of epic</param>
    /// <returns>List of epic</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssuesOfEpicByEpicIdAsync(Guid epicId);
    /// <summary>
    /// Get child issue of issue
    /// </summary>
    /// <param name="issueId">Id of issue</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssuesOfIssueByIssueIdAsync(Guid issueId);
    /// <summary>
    /// Load issue type of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadIssueTypeAsync(Issue issue);
    /// <summary>
    /// Load issue detail of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadIssueDetailAsync(Issue issue);
    /// <summary>
    /// Load attachmennts of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadAttachmentsAsync(Issue issue);
    /// <summary>
    /// Load status of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadStatusAsync(Issue issue);
    /// <summary>
    /// Get parent name of issue
    /// </summary>
    /// <param name="parentId">If of issue parent</param>
    /// <returns></returns>
    Task<string> GetParentNameAsync(Guid parentId);
    /// <summary>
    /// Get issue resolved a wwek ago
    /// </summary>
    /// <returns>List of issue</returns>
    /// <summary>
    /// 
    /// </summary>
    /// <param name="versionId">Id of version</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetIssuesOfVersionByVersionIdAsync(Guid versionId);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprintId">Id of sprint</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetNotDoneIssuesBySprintIdAsync(Guid sprintId, Guid projectId);
    Task<string> GetNameOfIssueAsync(Guid issueId);
    Task DeleteByBacklogIdAsync(Guid backlogId);
    Task DeleteBySprintIdAsync(Guid sprintId);
    Task DeleteByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<Guid>?> GetAllWatcherOfIssueAsync(Guid issueId);
    Task<string> GetProjectNameOfIssueAsync(Guid issueId);
    Task UpdateOneColumnForIssueAsync(Guid oldValue, Guid? newValue, NameColumn nameColumn);
    Task<int> CountIssueByPriorityIdAsync(Guid priorityId);
    Task<int> CountIssueByStatusIdAsync(Guid statusId);
    Task<int> CountIssueByIssueTypeIdAsync(Guid issueTypeId);
    Task<Guid> GetProjectIdOfIssueAsync(Guid issueId);
    Task<Guid> GetProjectLeadIdOfIssueAsync(Guid issueId);
    Task<string> GetProjectCodeOfIssueAsync(Guid issueId);
    void Insert(Issue issue);
    void Remove(Issue issue);
    void Update(Issue issue);
    Task<Project> GetProjectByIssueIdAsync(Guid issueId);
    void UpdateRange(IReadOnlyCollection<Issue> issues);
    void RemoveRange(IReadOnlyCollection<Issue> issues);
    Task DeleteChildIssueAsync(Guid parentId);
}
