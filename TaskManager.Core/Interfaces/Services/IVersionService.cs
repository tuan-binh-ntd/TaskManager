namespace TaskManager.Core.Interfaces.Services;

public interface IVersionService
{
    Task<IReadOnlyCollection<VersionViewModel>> GetByProjectId(Guid projectId);
    Task<VersionViewModel> Create(CreateVersionDto createVersionDto);
    Task<VersionViewModel> Update(Guid id, UpdateVersionDto updateVersionDto);
    Task<Guid> Delete(Guid id, Guid? newVersionId);
    Task<VersionViewModel> AddIssues(AddIssuesToVersionDto addIssuesToVersionDto);
    Task<GetIssuesByVersionIdViewModel> GetIssuesByVersionId(Guid versionId);
}
