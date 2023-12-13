using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface ILabelService
{
    Task<object> GetLabelsByProjectId(Guid projectId, PaginationInput paginationInput);
    Task<LabelViewModel> Create(Guid projectId, CreateLabelDto createLabelDto);
    Task<LabelViewModel> Update(Guid id, UpdateLabelDto updateLabelDto);
    Task<Guid> Delete(Guid id);
}
