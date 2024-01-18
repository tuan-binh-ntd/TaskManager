namespace TaskManager.Core.Interfaces.Services;

public interface IEpicService
{
    Task<EpicViewModel> AddIssueToEpic(Guid issueId, Guid epicId);
    Task<RealtimeNotificationViewModel> CreateEpic(CreateEpicDto createEpicDto);
    Task<GetIssuesByEpicIdViewModel> GetIssuesByEpicId(Guid epicId);
    Task<RealtimeNotificationViewModel> UpdateEpic(Guid id, UpdateEpicDto updateEpicDto);
    Task<RealtimeNotificationViewModel> DeleteEpic(Guid id, Guid userId);
}
