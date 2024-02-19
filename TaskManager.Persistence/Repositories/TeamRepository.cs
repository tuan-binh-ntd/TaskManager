namespace TaskManager.Persistence.Repositories;

public class TeamRepository(IDbContext context) : GenericRepository<Team>(context)
    , ITeamRepository
{
    public async Task<IReadOnlyCollection<Team>> GetTeamsByUserIdAsync(Guid userId)
    {
        var teams = await (from ut in DbContext.Set<UserTeam>().AsNoTracking().Where(t => t.UserId == userId)
                           join t in Entity.AsNoTracking() on ut.TeamId equals t.Id
                           select t)
                           .ToListAsync();
        return teams;
    }

    public async Task LoadEntitiesRelationshipAsync(Team team)
    {
        await Entity.Entry(team).Collection(i => i.UserTeams!).LoadAsync();
    }

    public async Task<IReadOnlyCollection<AppUser>> GetMembersByTeamIdAsync(Guid teamId)
    {
        var teams = await (from ut in DbContext.Set<UserTeam>().AsNoTracking().Where(t => t.TeamId == teamId)
                           join t in Entity.AsNoTracking() on ut.TeamId equals t.Id
                           select ut.User)
                           .ToListAsync();

        return teams.AsReadOnly();
    }
}
