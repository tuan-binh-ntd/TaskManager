using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;

        private readonly IMapper _mapper;

        public PriorityService(IPriorityRepository priorityRepository, IMapper mapper)
        {
            _priorityRepository = priorityRepository;
            _mapper = mapper;
        }

        public async Task<PriorityViewModel> Create(CreatePriorityDto createPriorityDto)
        {
            var priority = _mapper.Map<Priority>(createPriorityDto);
            _priorityRepository.Add(priority);
            await _priorityRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<PriorityViewModel>(priority);
        }

        public async Task<Guid> Delete(Guid id)
        {
            _priorityRepository.Delete(id);
            await _priorityRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<PriorityViewModel> GetById(Guid id)
        {
            var priority = await _priorityRepository.GetById(id);
            return _mapper.Map<PriorityViewModel>(priority);
        }

        public async Task<object> GetByProjectId(Guid projectId, PaginationInput paginationInput)
        {
            if (paginationInput.pagenum is not default(int) && paginationInput.pagesize is not default(int))
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
            var priority = await _priorityRepository.GetById(id);
            if (priority is null)
            {
                throw new ArgumentNullException(nameof(priority));
            }
            priority = _mapper.Map<Priority>(updatePriorityDto);
            _priorityRepository.Update(priority);
            await _priorityRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<PriorityViewModel>(priority);
        }
    }
}
