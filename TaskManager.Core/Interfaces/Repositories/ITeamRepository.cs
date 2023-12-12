using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ITeamRepository : IRepository<Team>
{
    Task<IReadOnlyCollection<Team>> GetByUserId(Guid userId);
    Task<Team> GetById(Guid id);
    Team Add(Team team);
    void Update(Team team);
    void Delete(Guid id);
    void LoadEntitiesRelationship(Team team);
    Task<IReadOnlyCollection<AppUser>> GetMembers(Guid teamId);
}
