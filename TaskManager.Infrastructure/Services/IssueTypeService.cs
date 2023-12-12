using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class IssueTypeService : IIssueTypeService
{
    private readonly IIssueTypeRepository _issueTypeRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IMapper _mapper;

    public IssueTypeService(
        IIssueTypeRepository issueTypeRepository,
        IIssueRepository issueRepository,
        IMapper mapper
        )
    {
        _issueTypeRepository = issueTypeRepository;
        _issueRepository = issueRepository;
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

    public async Task<Guid> Delete(Guid issueTypeId, Guid newIssueTypeId)
    {
        int count = await _issueRepository.CountIssueByIssueTypeId(issueTypeId);
        if (count > 0)
        {
            await _issueRepository.UpdateOneColumnForIssue(issueTypeId, newIssueTypeId);
        }
        _issueTypeRepository.Delete(issueTypeId);
        await _issueTypeRepository.UnitOfWork.SaveChangesAsync();
        return issueTypeId;
    }

    public async Task<object> GetIssueTypesByProjectId(Guid projectId, PaginationInput paginationInput)
    {
        if (paginationInput.pagenum is not default(int) && paginationInput.pagesize is not default(int))
        {
            var issueTypes = await _issueTypeRepository.GetsByProjectIdPaging(projectId, paginationInput);
            return issueTypes;
        }
        else
        {
            var issuesTypes = await _issueTypeRepository.GetsByProjectId(projectId);
            return issuesTypes;
        }
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
