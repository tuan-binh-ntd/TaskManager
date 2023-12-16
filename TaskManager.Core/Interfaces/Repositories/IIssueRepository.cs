using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueRepository : IRepository<Issue>
{
    /// <summary>
    /// Add issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    /// <returns>Issue entity</returns>
    Issue Add(Issue issue);
    /// <summary>
    /// Update issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    void Update(Issue issue);
    /// <summary>
    /// Delete issue by id
    /// </summary>
    /// <param name="id">Id of issue entity</param>
    void Delete(Guid id);
    /// <summary>
    /// Get issue by id
    /// </summary>
    /// <param name="id">Id of issue entity</param>
    /// <returns>Issue entity</returns>
    Task<Issue> Get(Guid id);
    /// <summary>
    /// Get all issue for sprint
    /// </summary>
    /// <param name="sprintId">Id of sprint entity</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetIssueBySprintId(Guid sprintId);
    /// <summary>
    /// Get all issue for backlog
    /// </summary>
    /// <param name="backlogId">Id of backlog entity</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetIssueByBacklogId(Guid backlogId);
    /// <summary>
    /// Update range issue
    /// </summary>
    /// <param name="issues">List of issue</param>
    void UpdateRange(IReadOnlyCollection<Issue> issues);
    /// <summary>
    /// Get all child of issue
    /// </summary>
    /// <param name="parentId">Id of parent issue entity</param>
    /// <returns>List of child issue</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssues(Guid parentId);
    /// <summary>
    /// Remove range issue
    /// </summary>
    /// <param name="issues">List of issue</param>
    void DeleteRange(IReadOnlyCollection<Issue> issues);
    /// <summary>
    /// Load full related entities to issue
    /// Use Explicit Loading
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadEntitiesRelationship(Issue issue);
    /// <summary>
    /// Get issues by id list
    /// </summary>
    /// <param name="ids">List of id</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetByIds(IReadOnlyCollection<Guid> ids);
    /// <summary>
    /// Get epic by projectId
    /// </summary>
    /// <param name="projectId">Id of project</param>
    /// <returns>List of epic</returns>
    Task<IReadOnlyCollection<Issue>> GetEpicByProjectId(Guid projectId);
    /// <summary>
    /// Get child issue of epic
    /// </summary>
    /// <param name="epicId">Id of epic</param>
    /// <returns>List of epic</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssueOfEpic(Guid epicId);
    /// <summary>
    /// Get child issue of issue
    /// </summary>
    /// <param name="issueId">Id of issue</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssueOfIssue(Guid issueId);
    /// <summary>
    /// Load comments of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadComments(Issue issue);
    /// <summary>
    /// Load issue type of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadIssueType(Issue issue);
    /// <summary>
    /// Load issue detail of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadIssueDetail(Issue issue);
    /// <summary>
    /// Load attachmennts of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadAttachments(Issue issue);
    /// <summary>
    /// Load issue histories of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadIssueHistories(Issue issue);
    /// <summary>
    /// Load status of issue
    /// </summary>
    /// <param name="issue">Issue entity</param>
    Task LoadStatus(Issue issue);
    /// <summary>
    /// Get parent name of issue
    /// </summary>
    /// <param name="parentId">If of issue parent</param>
    /// <returns></returns>
    Task<string> GetParentName(Guid parentId);
    /// <summary>
    /// Get issue created a week ago
    /// </summary>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetCreatedAWeekAgo();
    /// <summary>
    /// Get issue resolved a wwek ago
    /// </summary>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetResolvedAWeekAgo();
    /// <summary>
    /// Get issue updated a week ago
    /// </summary>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetUpdatedAWeekAgo();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="versionId">Id of version</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetChildIssueOfVersion(Guid versionId);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprintId">Id of sprint</param>
    /// <returns>List of issue</returns>
    Task<IReadOnlyCollection<Issue>> GetNotDoneIssuesBySprintId(Guid sprintId, Guid projectId);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprintIds"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Issue>> GetBySprintIds(IReadOnlyCollection<Guid> sprintIds, GetSprintByFilterDto getSprintByFilterDto, Guid projectId);
    Task<string?> GetNameOfIssue(Guid issueId);
    Task DeleteByBacklogId(Guid backlogId);
    Task DeleteBySprintId(Guid sprintId);
    Task DeleteByProjectId(Guid projectId);
    Task<IReadOnlyCollection<Guid>?> GetAllWatcherOfIssue(Guid issueId);
    Task<string> GetProjectNameOfIssue(Guid issueId);
    Task UpdateOneColumnForIssue(Guid oldValue, Guid? newValue);
    Task<int> CountIssueByPriorityId(Guid priorityId);
    Task<int> CountIssueByStatusId(Guid statusId);
    Task<int> CountIssueByIssueTypeId(Guid issueTypeId);
    Task<Guid> GetProjectIdOfIssue(Guid issueId);
    Task<Guid> GetProjectLeadIdOfIssue(Guid issueId);
}
