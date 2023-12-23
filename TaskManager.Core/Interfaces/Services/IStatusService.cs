using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IStatusService
{
    Task<StatusViewModel> Create(CreateStatusDto createStatusDto);
    Task<StatusViewModel> Update(Guid id, UpdateStatusDto updateStatusDto);
    Task<Guid> Delete(Guid id, Guid newId);
    Task<object> Gets(Guid projectId, PaginationInput paginationInput);
    Task<IReadOnlyCollection<StatusCategoryViewModel>> GetStatusCategoryViewModels();
    Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsForViewAsync(Guid projectId);
}
