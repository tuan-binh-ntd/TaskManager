using Ardalis.GuardClauses;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;

        public RoleService(
            IMapper mapper,
            RoleManager<AppRole> roleManager,
            IPermissionRepository permissionRepository
            )
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
        }

        public async Task<RoleViewModel> Create(CreateAppRoleDto appRoleDto)
        {
            AppRole appRole = new()
            {
                Name = appRoleDto.Name,
                ProjectId = appRoleDto.ProjectId,
            };
            await _roleManager.CreateAsync(appRole);
            return _mapper.Map<RoleViewModel>(appRole);
        }

        public async Task<PermissionViewModel> CreatePermission(CreatePermissionDto createPermissionDto)
        {
            var permission = createPermissionDto.Adapt<Permission>();
            _permissionRepository.Add(permission);
            await _permissionRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<PermissionViewModel>(permission);
        }

        public async Task<Guid> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            Guard.Against.Null(role, nameof(role));
            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded ? id : Guid.Empty;
        }

        public async Task<Guid> DeletePermission(Guid id)
        {
            _permissionRepository.Delete(id);
            await _permissionRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<RoleViewModel> Get(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(role));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            return _mapper.Map<RoleViewModel>(role);
        }

        public async Task<IReadOnlyCollection<RoleViewModel>> GetByProjectId(Guid projectId)
        {
            var roles = await _roleManager.Roles.Where(r => r.ProjectId == projectId).ToListAsync();
            var roleViewModels = roles.Adapt<IReadOnlyCollection<RoleViewModel>>();
            return roleViewModels;
        }

        public async Task<RoleViewModel> Update(Guid id, CreateAppRoleDto appRoleDto)
        {
            AppRole? role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(role));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            role.Name = appRoleDto.Name;
            role.ProjectId = appRoleDto.ProjectId;
            await _roleManager.UpdateAsync(role);
            return _mapper.Map<RoleViewModel>(role);
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
