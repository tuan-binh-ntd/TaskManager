using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ILabelRepository : IRepository<Label>
{
    Task<IReadOnlyCollection<Label>> GetByProjectId(Guid projectId);
    Task<Label?> GetById(Guid id);
    void Add(Label label);
    void Update(Label label);
    void Delete(Label label);
    void AddLabelIssue(LabelIssue labelIssue);
    void RemoveLabelIssue(LabelIssue labelIssue);
    Task<LabelIssue?> GetById(Guid labelId, Guid issueId);
    Task<IReadOnlyCollection<LabelViewModel>> GetByIssueId(Guid issueId);
    Task<PaginationResult<LabelViewModel>> GetByProjectId(Guid projectId, PaginationInput paginationInput);
    void AddRange(IReadOnlyCollection<LabelIssue> labelIssues);
}
