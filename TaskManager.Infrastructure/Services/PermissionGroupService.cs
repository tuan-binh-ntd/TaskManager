using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class PermissionGroupService : IPermissionGroupService
    {
        private readonly IMapper _mapper;

        private readonly IPermissionGroupRepository _permissionGroupRepository;

        public PermissionGroupService(IMapper mapper, IPermissionGroupRepository permissionGroupRepository)
        {
            _mapper = mapper;
            _permissionGroupRepository = permissionGroupRepository;
        }

        public async Task<PermissionGroupViewModel> Create(CreatePermissionGroupDto createPermissionGroupDto)
        {
            var permissionGroup = _mapper.Map<PermissionGroup>(createPermissionGroupDto);
            _permissionGroupRepository.Add(permissionGroup);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
            return permissionGroup.Adapt<PermissionGroupViewModel>();
        }

        public async Task<Guid> Delete(Guid id)
        {
            _permissionGroupRepository.Delete(id);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<IReadOnlyCollection<PermissionGroupViewModel>> GetPermissionGroupsByProjectId(Guid projectId)
        {
            var permissionGroups = await _permissionGroupRepository.GetByProjectId(projectId);
            return permissionGroups;
        }

        public async Task<PermissionGroupViewModel> Update(Guid id, UpdatePermissionGroupDto updatePermissionGroupDto)
        {
            var permissionGroup = await _permissionGroupRepository.GetById(id);
            if (permissionGroup is null)
            {
                throw new PermissionGroupNullException();
            }
            permissionGroup = _mapper.Map<PermissionGroup>(updatePermissionGroupDto);
            _permissionGroupRepository.Update(permissionGroup);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
            return permissionGroup.Adapt<PermissionGroupViewModel>();
        }
    }
}
