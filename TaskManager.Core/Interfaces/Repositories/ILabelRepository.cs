namespace TaskManager.Core.Interfaces.Repositories;

public interface ILabelRepository
{
    Task<IReadOnlyCollection<Label>> GetLabelsByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<LabelViewModel>> GetLabelViewModelsByIssueIdAsync(Guid issueId);
    Task<PaginationResult<LabelViewModel>> GetLabelViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput);
    void Insert(Label label);
    Task<Label?> GetByIdAsync(Guid id);
    void Update(Label label);
    void Remove(Label label);
}
