using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<PermissionViewModel> CreatePermission(CreatePermissionDto createPermissionDto)
        {
            var permission = createPermissionDto.Adapt<Permission>();
            _permissionRepository.Add(permission);
            await _permissionRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<PermissionViewModel>(permission);
        }

        public async Task<Guid> DeletePermission(Guid id)
        {
            var permission = await _permissionRepository.GetById(id);
            await _permissionRepository.LoadPermissionRoles(permission);
            _permissionRepository.Delete(permission);
            await _permissionRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<IReadOnlyCollection<PermissionViewModel>> GetPermissionViewModels()
        {
            var permissions = await _permissionRepository.GetAll();
            return permissions.Adapt<IReadOnlyCollection<PermissionViewModel>>();
        }

        public async Task<PermissionViewModel> UpdatePermission(Guid id, UpdatePermissionDto updatePermissionDto)
        {
            var permission = await _permissionRepository.GetById(id);
            permission.Name = updatePermissionDto.Name;
            permission.ParentId = updatePermissionDto.ParentId;
            _permissionRepository.Update(permission);
            await _permissionRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<PermissionViewModel>(permission);
        }
    }
}
