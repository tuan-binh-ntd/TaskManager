using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public FilterRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Filter Add(Filter filter)
        {
            return _context.Filters.Add(filter).Entity;
        }

        public void AddRange(IReadOnlyCollection<Filter> filters)
        {
            _context.Filters.AddRange(filters);
        }

        public void Delete(Guid id)
        {
            var filter = _context.Filters.FirstOrDefault(filter => filter.Id == id);
            if (filter != null)
            {
                _context.Filters.Remove(filter);
            }
        }

        public async Task<Filter> GetById(Guid id)
        {
            var filter = await _context.Filters.AsNoTracking().FirstOrDefaultAsync(filter => filter.Id == id);
            return filter!;
        }

        public async Task<IReadOnlyCollection<Filter>> GetByUserId(Guid userId)
        {
            var filters = await (from f in _context.Filters.AsNoTracking()
                                 join uf in _context.UserFilters.AsNoTracking().Where(uf => uf.UserId == userId) on f.Id equals uf.FilterId
                                 select f).ToListAsync();
            return filters.AsReadOnly();
        }

        public void Update(Filter filter)
        {
            _context.Entry(filter).State = EntityState.Modified;
        }

        public async Task<Filter> GetByName(string name)
        {
            var filter = await _context.Filters
                .Include(f => f.FilterCriterias)!
                .ThenInclude(fc => fc.Criteria)
                .AsNoTracking()
                .FirstOrDefaultAsync(filter => filter.Name == name);
            return filter!;
        }
    }
}
