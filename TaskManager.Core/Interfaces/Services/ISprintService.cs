using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface ISprintService
    {
        Task<SprintViewModel> CreateSprint(CreateSprintDto createSprintDto);
        Task<SprintViewModel> UpdateSprint(Guid id, UpdateSprintDto updateSprintDto);
        Task<SprintViewModel> CreateNoFieldSprint(Guid projectId);
        Task<Guid> DeleteSprint(Guid id);
        Task<SprintViewModel> StartSprint(Guid projectId, Guid sprintId, UpdateSprintDto updateSprintDto);
        Task<SprintViewModel> CompleteSprint(Guid sprintId, Guid projectId, CompleteSprintDto completeSprintDto);
        Task<SprintViewModel> GetById(Guid sprintId);
    }
}
