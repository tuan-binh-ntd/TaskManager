using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface ISprintService
    {
        Task<SprintViewModel> CreateSprint(CreateSprintDto createSprintDto);
        Task<SprintViewModel> UpdateSprint(Guid id, UpdateSprintDto updateSprintDto);
        Task<SprintViewModel> CreateNoFieldSprint(Guid projectId);
    }
}
