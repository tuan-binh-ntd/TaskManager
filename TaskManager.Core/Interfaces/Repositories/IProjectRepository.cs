namespace TaskManager.Core.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<IReadOnlyCollection<Project>> GetAll();
    Task<Project?> GetById(Guid id);
    Project Add(Project project);
    void Update(Project project);
    void Delete(Project project);
    Task<PaginationResult<Project>> GetByUserId(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput);
    Task<IReadOnlyCollection<Project>> GetByUserId(Guid userId, GetProjectByFilterDto input);
    Task<IReadOnlyCollection<UserViewModel>> GetMembers(Guid projectId);
    Task<Project?> GetByCode(string code);
    Task LoadIssueTypes(Project project);
    Task LoadStatuses(Project project);
    Task LoadBacklog(Project project);
    Task LoadUserProjects(Project project);
    Task LoadProjectConfiguration(Project project);
    Task LoadTransition(Project project);
    Task LoadWorkflow(Project project);
    Task LoadPriorities(Project project);
    Task LoadPermissionGroup(Project project);
    Task LoadSprints(Project project);
    Task LoadVersions(Project project);
    Task<string> GetProjectName(Guid projectId);
    Task<IReadOnlyCollection<SprintFilterViewModel>> GetSprintFiltersByProjectId(Guid projectId);
    Task<IReadOnlyCollection<TypeFilterViewModel>> GetIssueTypeFiltersByProjectId(Guid projectId);
    Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersByProjectId(Guid projectId);
    Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersByProjectId(Guid projectId);
    Task<Guid> GetUserIdByMemberId(Guid memberId);
}
