namespace TaskManager.Core.Interfaces.Services;

/// <summary>
/// Implementation business logic of project 
/// </summary>
public interface IProjectService
{
    Task<ProjectViewModel> Create(Guid userId, CreateProjectDto projectDto);
    Task<ProjectViewModel> Update(Guid userId, Guid projectId, UpdateProjectDto updateProjectDto);
    Task<Guid> Delete(Guid id);
    Task<object> GetProjectsByFilter(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput);
    Task<ProjectViewModel> Get(Guid projectId);
    Task<ProjectViewModel?> Get(string code, Guid userId);
    Task<ProjectViewModel> AddMember(AddMemberToProjectDto addMemberToProjectDto);
    Task<object> GetMembersOfProject(Guid projectId, PaginationInput paginationInput);
    Task<MemberProjectViewModel> UpdateMembder(Guid id, UpdateMemberProjectDto updateMemberProjectDto);
    Task<Guid> DeleteMember(Guid projectId, Guid id);
    Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersViewModel(Guid projectId);
    Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersViewModel(Guid projectId);
    Task<IReadOnlyCollection<TypeFilterViewModel>> GetTypeFiltersViewModel(Guid projectId);
    Task<IReadOnlyCollection<SprintFilterViewModel>> GetSprintFiltersViewModel(Guid projectId);
    Task<GetIssueForProjectViewModel> GetIssueForProjectViewModelAsync(Guid projectId, GetIssueForProjectFilterInputModel getIssueForProjectFilterInputModel);
    Task<IReadOnlyCollection<VersionFilterViewModel>> GetVerionFiltersViewModelForBacklog(Guid projectId);
    Task<IReadOnlyCollection<TypeFilterViewModel>> GetTypeFiltersViewModelForBacklog(Guid projectId);
    Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersViewModelForBacklog(Guid projectId);
    Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersViewModelForBacklog(Guid projectId);
}
