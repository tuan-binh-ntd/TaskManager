using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IUserProjectRepository : IRepository<UserProject>
{
    UserProject Add(UserProject userProject);
    void Update(UserProject userProject);
    UserProject? Get(Guid projectId, Guid userId);
    Task<object> GetMemberProjects(Guid projectId, PaginationInput paginationInput);
    Task<UserProject?> GetMember(Guid id);
    Task<MemberProjectViewModel?> GetMemberProject(Guid id);
    Task<IReadOnlyCollection<Guid>> GetByUserId(Guid userId);
    void Delete(UserProject userProject);
    Task<UserProject?> GetById(Guid id);
    Task<Guid> GetLeaderIdByProjectId(Guid projectId);
    Task UpdateIsFavouriteCol(Guid projectId, Guid userId, bool isFavourite);
    Task<bool> GetIsFavouriteCol(Guid projectId, Guid userId);
    Task<IReadOnlyCollection<Guid>> GetProjectIdsByUserId(Guid userId);
}
