using Mapster;
using MapsterMapper;
using TaskManager.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _statusRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IMapper _mapper;

    public StatusService(
        IStatusRepository statusRepository,
        IStatusCategoryRepository statusCategoryRepository,
        IIssueRepository issueRepository,
        IMapper mapper
        )
    {
        _statusRepository = statusRepository;
        _statusCategoryRepository = statusCategoryRepository;
        _issueRepository = issueRepository;
        _mapper = mapper;
    }

    public async Task<StatusViewModel> Create(CreateStatusDto createStatusDto)
    {
        var status = _mapper.Map<Status>(createStatusDto);
        _ = _statusRepository.Add(status);
        await _statusRepository.UnitOfWork.SaveChangesAsync();
        return status.Adapt<StatusViewModel>();
    }

    public async Task<Guid> Delete(Guid id, Guid newId)
    {
        int count = await _issueRepository.CountIssueByStatusId(id);
        if (count > 0)
        {
            await _issueRepository.UpdateOneColumnForIssue(id, newId, NameColumn.StatusId);
        }
        _statusRepository.Delete(id);
        await _statusRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<object> Gets(Guid projectId, PaginationInput paginationInput)
    {
        if (paginationInput.IsPaging())
        {
            var statuses = await _statusRepository.GetByProjectIdPaging(projectId, paginationInput);
            return statuses;
        }
        else
        {
            var statuses = await _statusRepository.GetByProjectId(projectId);
            return _mapper.Map<IReadOnlyCollection<StatusViewModel>>(statuses);
        }
    }

    public async Task<IReadOnlyCollection<StatusCategoryViewModel>> GetStatusCategoryViewModels()
    {
        var statusCategories = await _statusCategoryRepository.GetForStatus();
        return statusCategories.Adapt<IReadOnlyCollection<StatusCategoryViewModel>>();
    }

    public async Task<StatusViewModel> Update(Guid id, UpdateStatusDto updateStatusDto)
    {
        var status = await _statusRepository.GetById(id);
        status = updateStatusDto.Adapt(status);
        _statusRepository.Update(status);
        await _statusRepository.UnitOfWork.SaveChangesAsync();
        return status.Adapt<StatusViewModel>();
    }

    public async Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsForViewAsync(Guid projectId)
    {
        var statusViewModels = await _statusRepository.GetStatusViewModelsAsync(projectId);
        return statusViewModels;
    }
}
