namespace TaskManager.Core.Interfaces.Services;

public interface IIssueTypeService
{
    Task<object> GetIssueTypesByProjectId(Guid projectId, PaginationInput paginationInput);
    Task<IssueTypeViewModel> CreateIssueType(Guid projectId, CreateIssueTypeDto createIssueTypeDto);
    Task<IssueTypeViewModel> UpdateIssueType(Guid issueTypeId, UpdateIssueTypeDto updateIssueTypeDto);
    Task<Guid> Delete(Guid issueTypeId, Guid? newIssueTypeId);
}
