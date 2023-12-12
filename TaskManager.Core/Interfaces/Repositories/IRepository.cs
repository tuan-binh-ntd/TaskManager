using TaskManager.Core.Core;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IRepository<T> where T : IEntity
{
    IUnitOfWork UnitOfWork { get; }
}
