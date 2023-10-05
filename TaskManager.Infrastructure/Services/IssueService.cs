using Mapster;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IIssueHistoryRepository _issueHistoryRepository;

        public IssueService(
            IIssueRepository issueRepository,
            IIssueHistoryRepository issueHistoryRepository
            )
        {
            _issueRepository = issueRepository;
            _issueHistoryRepository = issueHistoryRepository;
        }

        public async Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto)
        {
            var issue = createIssueDto.Adapt<Issue>();

            var issueVM = _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueHis = new IssueHistory
            {
                Name = "created the Issue",
                CreatorUserId = createIssueDto.CreatorUserId,
                IssueId = issue.Id,
            };

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();
            return issueVM;
        }

        public async Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto)
        {
            var issue = _issueRepository.Get(id);
            if (issue is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(issue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            issue.Name = updateIssueDto.Name;
            issue.Description = updateIssueDto.Description;
            issue.CompleteDate = updateIssueDto.CompleteDate;
            issue.Priority = updateIssueDto.Priority;
            issue.Watcher = updateIssueDto.Watcher;
            issue.Voted = updateIssueDto.Voted;
            issue.StartDate = updateIssueDto.StartDate;
            issue.DueDate = updateIssueDto.DueDate;
            issue.ParentId = updateIssueDto.ParentId;
            issue.BacklogId = issue.BacklogId;
            issue.SprintId = issue.SprintId;
            issue.IssueTypeId = issue.IssueTypeId;
            _issueRepository.Update(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            return issue.Adapt<IssueViewModel>();
        }
        //public Task<IReadOnlyCollection<IssueDetailViewModel>> GetBySprintId(Guid sprintId)
        //{
        //    var issues = _issueRepository.Gets();
        //}
    }
}
