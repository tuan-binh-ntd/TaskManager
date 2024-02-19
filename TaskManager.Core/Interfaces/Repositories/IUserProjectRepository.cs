namespace TaskManager.Core.Interfaces.Repositories;

public interface IUserProjectRepository
{
    Task<object> GetMembersByProjectIdAsync(Guid projectId, PaginationInput paginationInput);
    Task<MemberProjectViewModel?> GetMemberProjectViewModelByIdAsync(Guid id);
    Task<Guid> GetLeaderIdByProjectIdAsync(Guid projectId);
    Task UpdateIsFavouriteColAsync(Guid projectId, Guid userId, bool isFavourite);
    Task<bool> GetIsFavouriteColAsync(Guid projectId, Guid userId);
    Task<IReadOnlyCollection<Guid>> GetProjectIdsByUserIdAsync(Guid userId);
    Task<IReadOnlyCollection<UserProject>> GetUserProjectsByPermissionGroupIdAsync(Guid permissionGroupId);
    Task UpdatePermissionGroupIdAsync(Guid oldValue, Guid newValue);
    Task<Guid> GetUserIdById(Guid memberId);
    Task<UserProject?> GetByIdAsync(Guid id);
    void Insert(UserProject userProject);
    void Update(UserProject userProject);
    void Remove(UserProject userProject);
    void InsertRange(IReadOnlyCollection<UserProject> userProjects);
}
