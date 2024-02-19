namespace TaskManager.Core.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<PaginationResult<Project>> GetProjectsByUserIdPagingAsync(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput);
    Task<IReadOnlyCollection<Project>> GetProjectsByUserIdAsync(Guid userId, GetProjectByFilterDto input);
    Task<IReadOnlyCollection<UserViewModel>> GetMembersAsync(Guid projectId);
    Task<Project?> GetProjectByCodeAsync(string code);
    Task LoadIssueTypesAsync(Project project);
    Task LoadStatusesAsync(Project project);
    Task LoadBacklogAsync(Project project);
    Task LoadUserProjectsAsync(Project project);
    Task LoadProjectConfigurationAsync(Project project);
    Task LoadTransitionAsync(Project project);
    Task LoadWorkflowAsync(Project project);
    Task LoadPrioritiesAsync(Project project);
    Task LoadPermissionGroupsAsync(Project project);
    Task LoadSprintsAsync(Project project);
    Task LoadVersionsAsync(Project project);
    Task<string> GetProjectNameAsync(Guid projectId);
    Task<IReadOnlyCollection<SprintFilterViewModel>> GetSprintFiltersByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<TypeFilterViewModel>> GetIssueTypeFiltersByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersByProjectIdAsync(Guid projectId);
    Task<Project?> GetByIdAsync(Guid id);
    void Insert(Project project);
    void Update(Project project);
    void Remove(Project project);
}
