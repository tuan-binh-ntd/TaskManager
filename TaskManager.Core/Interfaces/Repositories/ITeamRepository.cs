namespace TaskManager.Core.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<IReadOnlyCollection<Team>> GetTeamsByUserIdAsync(Guid userId);
    Task LoadEntitiesRelationshipAsync(Team team);
    Task<IReadOnlyCollection<AppUser>> GetMembersByTeamIdAsync(Guid teamId);
}
