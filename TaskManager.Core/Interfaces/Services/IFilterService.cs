using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IFilterService
{
    Task<FilterViewModel> CreateFilter(CreateFilterDto createFilterDto);
    Task<FilterViewModel> UpdateFilter(Guid id, UpdateFilterDto updateFilterDto);
    Task<Guid> DeleteFilter(Guid id);
    /// <summary>
    /// Get all issue
    /// </summary>
    /// <param name="id">Id of filter</param>
    /// <returns>List of issues</returns>
    Task<IReadOnlyCollection<IssueViewModel>> GetIssueByFilterConfiguration(Guid id, Guid userId);
    Task<IReadOnlyCollection<IssueViewModel>> GetIssuesByConfiguration(GetIssueByConfigurationDto getIssueByConfigurationDto);
    Task<IReadOnlyCollection<FilterViewModel>> GetFilterViewModelsByUserId(Guid userId);
    Task<FilterViewModel> GetFilterViewModelById(Guid id);
}
