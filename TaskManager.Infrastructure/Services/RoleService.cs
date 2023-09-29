using Ardalis.GuardClauses;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(
            IMapper mapper,
            RoleManager<AppRole> roleManager
            )
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<RoleViewModel> Create(AppRoleDto appRoleDto)
        {
            AppRole appRole = new()
            {
                Name = appRoleDto.Name,
            };
            await _roleManager.CreateAsync(appRole);
            return _mapper.Map<RoleViewModel>(appRole);
        }

        public async Task<bool> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            Guard.Against.Null(role, nameof(role));
            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded;
        }

        public async Task<RoleViewModel> Get(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return _mapper.Map<RoleViewModel>(role);
        }

        public async Task<IReadOnlyCollection<RoleViewModel>> Gets()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = _mapper.Map<List<RoleViewModel>>(roles);
            return roleViewModels.AsReadOnly();
        }

        public async Task<RoleViewModel> Update(Guid id, AppRoleDto appRoleDto)
        {
            AppRole? role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(role));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            role.Name = appRoleDto.Name;
            await _roleManager.UpdateAsync(role);
            return _mapper.Map<RoleViewModel>(role);
        }
    }
}
