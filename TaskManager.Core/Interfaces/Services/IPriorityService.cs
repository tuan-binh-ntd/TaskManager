using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IPriorityService
    {
        Task<IReadOnlyCollection<PriorityViewModel>> GetByProjectId(Guid projectId);
        Task<PriorityViewModel> GetById(Guid id);
        Task<PriorityViewModel> Create(CreatePriorityDto createPriorityDto);
        Task<PriorityViewModel> Update(Guid id, UpdatePriorityDto updatePriorityDto);
        Task<Guid> Delete(Guid id);
    }
}
