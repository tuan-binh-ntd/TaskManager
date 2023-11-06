using Dapper;
using MapsterMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class FilterService : IFilterService
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public FilterService(
            IIssueRepository issueRepository,
            IOptionsMonitor<ConnectionStrings> optionsMonitor,
            IMapper mapper
            )
        {
            _connectionStrings = optionsMonitor.CurrentValue;
            _issueRepository = issueRepository;
            _mapper = mapper;
        }
        #region Private Method
        private async Task<IReadOnlyCollection<IssueViewModel>> ToIssueViewModels(IReadOnlyCollection<Issue> issues)
        {
            var issueViewModels = new List<IssueViewModel>();
            if (issues.Any())
            {
                foreach (var issue in issues)
                {
                    var issueViewModel = await ToIssueViewModel(issue);
                    issueViewModels.Add(issueViewModel);
                }
            }
            return issueViewModels.AsReadOnly();
        }

        private async Task<IssueViewModel> ToIssueViewModel(Issue issue)
        {
            _issueRepository.LoadEntitiesRelationship(issue);
            var issueViewModel = _mapper.Map<IssueViewModel>(issue);
            var childIssues = await _issueRepository.GetChildIssueOfIssue(issue.Id);
            if (issue.IssueDetail is not null)
            {
                var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
                issueViewModel.IssueDetail = issueDetail;
            }
            if (issue.IssueHistories is not null && issue.IssueHistories.Any())
            {
                var issueHistories = _mapper.Map<ICollection<IssueHistoryViewModel>>(issue.IssueHistories);
                issueViewModel.IssueHistories = issueHistories;
            }
            if (issue.Comments is not null && issue.Comments.Any())
            {
                var comments = _mapper.Map<ICollection<CommentViewModel>>(issue.Comments);
                issueViewModel.Comments = comments;
            }
            if (issue.Attachments is not null && issue.Attachments.Any())
            {
                var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(issue.Attachments);
                issueViewModel.Attachments = attachments;
            }
            if (issue.IssueType is not null)
            {
                var issueType = _mapper.Map<IssueTypeViewModel>(issue.IssueType);
                issueViewModel.IssueType = issueType;
            }
            if (issue.Status is not null)
            {
                var status = _mapper.Map<StatusViewModel>(issue.Status);
                issueViewModel.Status = status;
            }
            if (issue.ParentId is not null)
            {
                issueViewModel.ParentName = await _issueRepository.GetParentName(issue.Id);
            }
            if (childIssues.Any())
            {
                issueViewModel.ChildIssues = _mapper.Map<ICollection<ChildIssueViewModel>>(childIssues);
            }
            return issueViewModel;
        }
        #endregion
        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByMyOpenIssuesFilter(Guid userId)
        {
            string query = @"
                SELECT 
                  IssueId Id
                FROM IssueDetails id
                JOIN Issues i ON id.IssueId = i.Id
                JOIN Statuses s ON i.StatusId = s.Id
                JOIN StatusCategories sc ON s.StatusCategoryId = sc.Id
                WHERE sc.Code <> 'Done'
                  AND id.AssigneeId = @UserId
            ";

            var param = new
            {
                UserId = userId,
            };

            using SqlConnection connection = new(_connectionStrings.DefaultConnection);
            var issueIds = await connection.QueryAsync<Guid>(query, param);
            if (issueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(issueIds.ToList());
                return await ToIssueViewModels(issues);
            }
            else
            {
                return new List<IssueViewModel>();
            }
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByReportedByMeFilter(Guid userId)
        {
            string query = @"
                SELECT 
                  IssueId Id
                FROM IssueDetails
                WHERE ReporterId = @UserId
            ";

            var param = new
            {
                UserId = userId,
            };

            using SqlConnection connection = new(_connectionStrings.DefaultConnection);
            var issueIds = await connection.QueryAsync<Guid>(query, param);
            if (issueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(issueIds.ToList());
                return await ToIssueViewModels(issues);
            }
            else
            {
                return new List<IssueViewModel>();
            }
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByAllIssueFilter(Guid userId)
        {
            string query = @"
                SELECT 
                  i.Id
                FROM UserProjects up
                JOIN Projects p ON up.ProjectId = p.Id
                JOIN Backlogs b ON p.Id = b.ProjectId
                JOIN Sprints s ON p.Id = s.ProjectId
                JOIN Issues i ON s.Id = i.SprintId OR b.Id = i.BacklogId
                WHERE up.UserId = @UserId
            ";

            var param = new
            {
                UserId = userId,
            };

            using SqlConnection connection = new(_connectionStrings.DefaultConnection);
            var issueIds = await connection.QueryAsync<Guid>(query, param);
            if (issueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(issueIds.ToList());
                return await ToIssueViewModels(issues);
            }
            else
            {
                return new List<IssueViewModel>();
            }
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByOpenIssuesFilter()
        {
            string query = @"
                SELECT 
                  IssueId Id
                FROM IssueDetails id
                JOIN Issues i ON id.IssueId = i.Id
                JOIN Statuses s ON i.StatusId = s.Id
                JOIN StatusCategories sc ON s.StatusCategoryId = sc.Id
                WHERE sc.Code <> 'Done'
            ";

            using SqlConnection connection = new(_connectionStrings.DefaultConnection);
            var issueIds = await connection.QueryAsync<Guid>(query);
            if (issueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(issueIds.ToList());
                return await ToIssueViewModels(issues);
            }
            else
            {
                return new List<IssueViewModel>();
            }
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByDoneIssuesFilter()
        {
            string query = @"
                SELECT 
                  IssueId Id
                FROM IssueDetails id
                JOIN Issues i ON id.IssueId = i.Id
                JOIN Statuses s ON i.StatusId = s.Id
                JOIN StatusCategories sc ON s.StatusCategoryId = sc.Id
                WHERE sc.Code = 'Done'
            ";

            using SqlConnection connection = new(_connectionStrings.DefaultConnection);
            var issueIds = await connection.QueryAsync<Guid>(query);
            if (issueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(issueIds.ToList());
                return await ToIssueViewModels(issues);
            }
            else
            {
                return new List<IssueViewModel>();
            }
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByCreatedRecentlyFilter()
        {
            var issues = await _issueRepository.GetCreatedAWeekAgo();
            return await ToIssueViewModels(issues);
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByResolvedRecentlyFilter()
        {
            var issues = await _issueRepository.GetResolvedAWeekAgo();
            return await ToIssueViewModels(issues);
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetIssueByUpdatedRecentlyFilter()
        {
            var issues = await _issueRepository.GetUpdatedAWeekAgo();
            return await ToIssueViewModels(issues);
        }
    }
}
