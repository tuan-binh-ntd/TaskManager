using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class IssueTypeService : IIssueTypeService
    {
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly IMapper _mapper;

        public IssueTypeService(
            IIssueTypeRepository issueTypeRepository,
            IMapper mapper
            )
        {
            _issueTypeRepository = issueTypeRepository;
            _mapper = mapper;
        }

        public async Task<IssueTypeViewModel> CreateIssueType(Guid projectId, CreateIssueTypeDto createIssueTypeDto)
        {
            var issueType = _mapper.Map<IssueType>(createIssueTypeDto);
            issueType.ProjectId = projectId;
            issueType.Level = 2;
            var issueTypeViewModel = _issueTypeRepository.Add(issueType);
            await _issueTypeRepository.UnitOfWork.SaveChangesAsync();
            return issueTypeViewModel;
        }

        public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetIssueTypesByProjectId(Guid projectId)
        {
            var issuesTypes = await _issueTypeRepository.GetsByProjectId(projectId);
            return issuesTypes;
        }

        public async Task<IssueTypeViewModel> UpdateIssueType(Guid issueTypeId, UpdateIssueTypeDto updateIssueTypeDto)
        {
            IssueType issuesType = await _issueTypeRepository.Get(issueTypeId);
            issuesType = _mapper.Map<IssueType>(updateIssueTypeDto);
            _issueTypeRepository.Update(issuesType);
            await _issueTypeRepository.UnitOfWork.SaveChangesAsync();
            return issuesType.Adapt<IssueTypeViewModel>();
        }
    }
}
