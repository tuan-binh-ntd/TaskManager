﻿using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IIssueService
    {
        Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto, Guid? sprintId = default, Guid? backlogId = default);
        Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto);
        Task<IReadOnlyCollection<IssueViewModel>> GetBySprintId(Guid sprintId);
        Task<IReadOnlyCollection<IssueViewModel>> GetByBacklogId(Guid backlogId);
        Task<IssueViewModel> CreateIssueByName(CreateIssueByNameDto createIssueByNameDto, Guid? sprintId = default, Guid? backlogId = default);
        Task<Guid> DeleteIssue(Guid id);
        Task<ChildIssueViewModel> CreateChildIssue(CreateChildIssueDto createChildIssueDto);
        Task<IssueViewModel> AddEpic(Guid issueId, Guid epicId);
        Task<EpicViewModel> CreateEpic(CreateEpicDto createEpicDto);
        Task<IssueViewModel> GetById(Guid id);
    }
}
