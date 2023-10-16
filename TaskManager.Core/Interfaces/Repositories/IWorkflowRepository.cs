using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IWorkflowRepository : IRepository<Workflow>
    {
        Workflow Add(Workflow workflow);
        void Update(Workflow workflow);
        void Delete(Guid id);
    }
}
