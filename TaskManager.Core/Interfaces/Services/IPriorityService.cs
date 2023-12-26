using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IPriorityService
{
    Task<object> GetByProjectId(Guid projectId, PaginationInput paginationInput);
    Task<PriorityViewModel> GetById(Guid id);
    Task<PriorityViewModel> Create(CreatePriorityDto createPriorityDto);
    Task<PriorityViewModel> Update(Guid id, UpdatePriorityDto updatePriorityDto);
    Task<Guid> Delete(Guid id, Guid? newId);
}
