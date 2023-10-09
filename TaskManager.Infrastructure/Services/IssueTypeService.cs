using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class IssueTypeService : IIssueTypeService
    {
        private readonly IIssueTypeRepository _issueTypeRepository;

        public IssueTypeService(
            IIssueTypeRepository issueTypeRepository
            )
        {
            _issueTypeRepository = issueTypeRepository;
        }

        public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypesByProjectId(Guid projectId)
        {
            var issuesTypes = await _issueTypeRepository.GetsByProjectId(projectId);
            return issuesTypes;
        }
    }
}
