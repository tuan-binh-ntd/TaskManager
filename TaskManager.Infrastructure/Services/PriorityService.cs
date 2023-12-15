using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class PriorityService : IPriorityService
{
    private readonly IPriorityRepository _priorityRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IMapper _mapper;

    public PriorityService(
        IPriorityRepository priorityRepository,
        IIssueRepository issueRepository,
        IMapper mapper)
    {
        _priorityRepository = priorityRepository;
        _issueRepository = issueRepository;
        _mapper = mapper;
    }

    public async Task<PriorityViewModel> Create(CreatePriorityDto createPriorityDto)
    {
        var priority = _mapper.Map<Priority>(createPriorityDto);
        _priorityRepository.Add(priority);
        await _priorityRepository.UnitOfWork.SaveChangesAsync();
        return _mapper.Map<PriorityViewModel>(priority);
    }

    public async Task<Guid> Delete(Guid id, Guid newId)
    {
        int count = await _issueRepository.CountIssueByPriorityId(id);
        if (count > 0)
        {
            await _issueRepository.UpdateOneColumnForIssue(id, newId);
        }
        _priorityRepository.Delete(id);
        await _priorityRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<PriorityViewModel> GetById(Guid id)
    {
        var priority = await _priorityRepository.GetById(id) ?? throw new PriorityNullException();
        return _mapper.Map<PriorityViewModel>(priority);
    }

    public async Task<object> GetByProjectId(Guid projectId, PaginationInput paginationInput)
    {
        if (paginationInput.IsPaging())
        {
            var priorities = await _priorityRepository.GetByProjectId(projectId, paginationInput);
            return priorities;
        }
        else
        {
            var priorities = await _priorityRepository.GetByProjectId(projectId);
            return _mapper.Map<IReadOnlyCollection<PriorityViewModel>>(priorities);
        }
    }

    public async Task<PriorityViewModel> Update(Guid id, UpdatePriorityDto updatePriorityDto)
    {
        var priority = await _priorityRepository.GetById(id) ?? throw new PriorityNullException();
        priority = _mapper.Map<Priority>(updatePriorityDto);
        _priorityRepository.Update(priority);
        await _priorityRepository.UnitOfWork.SaveChangesAsync();
        return _mapper.Map<PriorityViewModel>(priority);
    }
}
