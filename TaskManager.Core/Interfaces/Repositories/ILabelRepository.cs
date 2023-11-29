using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ILabelRepository : IRepository<Label>
{
    Task<IReadOnlyCollection<Label>> GetByProjectId(Guid projectId);
    Task<Label?> GetById(Guid id);
    void Add(Label label);
    void Update(Label label);
    void Delete(Label label);
    void AddLabelIssue(LabelIssue labelIssue);
}
