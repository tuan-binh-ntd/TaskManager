using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class StatusCategoryRepository : IStatusCategoryRepository
    {
        private readonly AppDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public StatusCategoryRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IReadOnlyCollection<StatusCategory> Gets()
        {
            var statusCategories = _context.StatusCategories.AsNoTracking().ToList();
            return statusCategories.AsReadOnly();
        }

        public async Task<StatusCategory?> GetDone()
        {
            var doneStatusCategory = await _context.StatusCategories.Where(sc => sc.Code == CoreConstants.DoneCode).FirstOrDefaultAsync();
            return doneStatusCategory;
        }

        public async Task<IReadOnlyCollection<StatusCategory>> GetForStatus()
        {
            var statusCategoryCodes = new List<string>()
            {
                CoreConstants.ToDoCode,
                CoreConstants.InProgressCode,
                CoreConstants.DoneCode,
            };
            var statusCategories = await _context.StatusCategories.Where(sc => statusCategoryCodes.Contains(sc.Code)).AsNoTracking().ToListAsync();
            return statusCategories.AsReadOnly();
        }
    }
}
