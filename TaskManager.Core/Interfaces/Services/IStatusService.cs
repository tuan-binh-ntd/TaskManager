using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IStatusService
    {
        Task<StatusViewModel> Create(CreateStatusDto createStatusDto);
        Task<StatusViewModel> Update(Guid id, UpdateStatusDto updateStatusDto);
        Task<Guid> Delete(Guid id);
        Task<IReadOnlyCollection<StatusViewModel>> Gets(Guid projectId);
    }
}
