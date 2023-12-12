using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IEpicService
{
    Task<EpicViewModel> AddIssueToEpic(Guid issueId, Guid epicId);
    Task<EpicViewModel> CreateEpic(CreateEpicDto createEpicDto);
    Task<GetIssuesByEpicIdViewModel> GetIssuesByEpicId(Guid epicId);
    Task<EpicViewModel> UpdateEpic(Guid id, UpdateEpicDto updateEpicDto);
    Task<Guid> DeleteEpic(Guid id);
}
