using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IStatusService
    {
        Task<StatusViewModel> Create(CreateStatusDto createStatusDto);
        Task<StatusViewModel> Update(Guid id, UpdateStatusDto updateStatusDto);
        Task<Guid> Delete(Guid id);
        Task<object> Gets(Guid projectId, PaginationInput paginationInput);
    }
}
