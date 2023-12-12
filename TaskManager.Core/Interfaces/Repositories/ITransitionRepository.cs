using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ITransitionRepository : IRepository<Transition>
{
    Transition Add(Transition transition);
    void Update(Transition transition);
    void Delete(Guid id);
    void AddRange(ICollection<Transition> transitions);
    Transition GetCreateTransitionByProjectId(Guid projectId);
}
