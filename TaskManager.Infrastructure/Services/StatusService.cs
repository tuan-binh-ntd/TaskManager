using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;

        public StatusService(
            IStatusRepository statusRepository,
            IMapper mapper
            )
        {
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        public async Task<StatusViewModel> Create(CreateStatusDto createStatusDto)
        {
            var status = _mapper.Map<Status>(createStatusDto);
            _ = _statusRepository.Add(status);
            await _statusRepository.UnitOfWork.SaveChangesAsync();
            return status.Adapt<StatusViewModel>();
        }

        public async Task<Guid> Delete(Guid id)
        {
            _statusRepository.Delete(id);
            await _statusRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<IReadOnlyCollection<StatusViewModel>> Gets(Guid projectId)
        {
            var statuses = await _statusRepository.GetByProjectId(projectId);
            return _mapper.Map<IReadOnlyCollection<StatusViewModel>>(statuses);
        }

        public async Task<StatusViewModel> Update(Guid id, UpdateStatusDto updateStatusDto)
        {
            var status = await _statusRepository.GetById(id);
            status = updateStatusDto.Adapt(status);
            _statusRepository.Update(status);
            await _statusRepository.UnitOfWork.SaveChangesAsync();
            return status.Adapt<StatusViewModel>();
        }
    }
}
