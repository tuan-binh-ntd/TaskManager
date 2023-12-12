using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class CriteriaRepository : ICriteriaRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public CriteriaRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyCollection<Criteria>> Gets()
    {
        var criterias = await _context.Criterias.AsNoTracking().ToListAsync();
        return criterias.AsReadOnly();
    }
}
