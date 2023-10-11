using Mapster;
using MapsterMapper;
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
        private readonly IIssueDetailRepository _issueDetailRepository;
        private readonly IMapper _mapper;

        public IssueService(
            IIssueRepository issueRepository,
            IIssueHistoryRepository issueHistoryRepository,
            IIssueDetailRepository issueDetailRepository,
            IMapper mapper
            )
        {
            _issueRepository = issueRepository;
            _issueHistoryRepository = issueHistoryRepository;
            _issueDetailRepository = issueDetailRepository;
            _mapper = mapper;
        }

        public async Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto, Guid? sprintId = null, Guid? backlogId = null)
        {
            var issue = createIssueDto.Adapt<Issue>();

            if (sprintId is not null)
            {
                issue.SprintId = sprintId;
            }
            else
            {
                issue.BacklogId = backlogId;
            }

            _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            var issueVM = _mapper.Map<IssueViewModel>(issue);

            var issueDetail = new IssueDetail()
            {
                ReporterId = createIssueDto.CreatorUserId,
                StoryPointEstimate = 0,
                Label = string.Empty,
                IssueId = issue.Id,
            };

            _issueDetailRepository.Add(issueDetail);
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

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
            issue = updateIssueDto.Adapt(issue);
            _issueRepository.Update(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            return issue.Adapt<IssueViewModel>();
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetBySprintId(Guid sprintId)
        {
            var issues = await _issueRepository.GetIssueBySprintId(sprintId);
            return issues.Adapt<IReadOnlyCollection<IssueViewModel>>();
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetByBacklogId(Guid backlogId)
        {
            var issues = await _issueRepository.GetIssueByBacklogId(backlogId);
            return issues.Adapt<IReadOnlyCollection<IssueViewModel>>();
        }

        public async Task<IssueViewModel> CreateIssueByName(CreateIssueByNameDto createIssueByNameDto, Guid? sprintId = null, Guid? backlogId = null)
        {
            var issue = new Issue()
            { 
                Name = createIssueByNameDto.Name,
                IssueTypeId = createIssueByNameDto.IssueTypeId,
            };

            if (sprintId is not null)
            {
                issue.SprintId = sprintId;
            }
            else
            {
                issue.BacklogId = backlogId;
            }

            _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            var issueVM = _mapper.Map<IssueViewModel>(issue);

            var issueDetail = new IssueDetail()
            {
                ReporterId = createIssueByNameDto.CreatorUserId,
                StoryPointEstimate = 0,
                Label = string.Empty,
                IssueId = issue.Id,
            };

            _issueDetailRepository.Add(issueDetail);
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

            var issueHis = new IssueHistory
            {
                Name = "created the Issue",
                CreatorUserId = createIssueByNameDto.CreatorUserId,
                IssueId = issue.Id,
            };

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();
            return issueVM;
        }
    }
}
