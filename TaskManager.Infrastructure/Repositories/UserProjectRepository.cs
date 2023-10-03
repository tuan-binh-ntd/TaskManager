using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class UserProjectRepository : IUserProjectRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserProjectRepository(
            AppDbContext context
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UserProject Add(UserProject userProject)
        {
            return _context.UserProjects.Add(userProject).Entity;
        }
    }
}
