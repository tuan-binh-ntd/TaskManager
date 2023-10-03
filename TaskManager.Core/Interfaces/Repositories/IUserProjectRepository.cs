using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IUserProjectRepository : IRepository<UserProject>
    {
        UserProject Add(UserProject userProject);
    }
}
