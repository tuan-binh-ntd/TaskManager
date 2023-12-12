using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Team Add(Team team)
    {
        return _context.Teams.Add(team).Entity;
    }

    public void Delete(Guid id)
    {
        var team = _context.Teams.FirstOrDefault(t => t.Id == id);
        _context.Teams.Remove(team!);
    }

    public async Task<Team> GetById(Guid id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
        return team!;
    }

    public async Task<IReadOnlyCollection<Team>> GetByUserId(Guid userId)
    {
        var teams = await (from ut in _context.UserTeams.AsNoTracking().Where(t => t.UserId == userId)
                           join t in _context.Teams.AsNoTracking() on ut.TeamId equals t.Id
                           select t).ToListAsync();
        return teams;
    }

    public void Update(Team team)
    {
        _context.Entry(team).State = EntityState.Modified;
    }

    public void LoadEntitiesRelationship(Team team)
    {
        _context.Entry(team).Collection(i => i.UserTeams!).Load();
    }

    public async Task<IReadOnlyCollection<AppUser>> GetMembers(Guid teamId)
    {
        var teams = await (from ut in _context.UserTeams.AsNoTracking().Where(t => t.TeamId == teamId)
                           join t in _context.Teams.AsNoTracking() on ut.TeamId equals t.Id
                           select ut.User).ToListAsync();
        return teams.AsReadOnly();
    }
}
