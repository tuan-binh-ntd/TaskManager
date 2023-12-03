using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class PermissionGroupService : IPermissionGroupService
    {

        private readonly IPermissionGroupRepository _permissionGroupRepository;

        public PermissionGroupService(IPermissionGroupRepository permissionGroupRepository)
        {
            _permissionGroupRepository = permissionGroupRepository;
        }

        #region Private methods
        private static async Task<PermissionGroupViewModel> ToPermissionGroupViewModel(PermissionGroup permissionGroup)
        {
            var permissionGroupViewModel = new PermissionGroupViewModel()
            {
                Id = permissionGroup.Id,
                Name = permissionGroup.Name,
                Permissions = permissionGroup.Permissions.FromJson<Permissions>()
            };

            return await Task.FromResult(permissionGroupViewModel);
        }
        #endregion

        public async Task<PermissionGroupViewModel> Create(CreatePermissionGroupDto createPermissionGroupDto)
        {
            var permissions = new Permissions
            {
                Timeline = createPermissionGroupDto.Timeline,
                Backlog = createPermissionGroupDto.Backlog,
                Board = createPermissionGroupDto.Board,
                Project = createPermissionGroupDto.Project,
            };

            var permissionGroup = new PermissionGroup()
            {
                Name = createPermissionGroupDto.Name,
                ProjectId = createPermissionGroupDto.ProjectId,
                Permissions = permissions.ToJson(),
            };
            _permissionGroupRepository.Add(permissionGroup);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
            return await ToPermissionGroupViewModel(permissionGroup);
        }

        public async Task<Guid> Delete(Guid id)
        {
            _permissionGroupRepository.Delete(id);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<object> GetPermissionGroupsByProjectId(Guid projectId, PaginationInput paginationInput)
        {
            if (paginationInput.pagenum is not default(int) && paginationInput.pagesize is not default(int))
            {
                var permissionGroups = await _permissionGroupRepository.GetByProjectId(projectId, paginationInput);
                return permissionGroups;
            }
            else
            {
                var permissionGroups = await _permissionGroupRepository.GetByProjectId(projectId);
                return permissionGroups;
            }
        }

        public async Task<PermissionGroupViewModel> Update(Guid id, UpdatePermissionGroupDto updatePermissionGroupDto)
        {
            var permissionGroup = await _permissionGroupRepository.GetById(id) ?? throw new PermissionGroupNullException();
            var permissions = new Permissions
            {
                Timeline = updatePermissionGroupDto.Timeline,
                Backlog = updatePermissionGroupDto.Backlog,
                Board = updatePermissionGroupDto.Board,
                Project = updatePermissionGroupDto.Project,
            };

            permissionGroup.Name = string.IsNullOrWhiteSpace(updatePermissionGroupDto.Name) ? permissionGroup.Name : updatePermissionGroupDto.Name;
            permissionGroup.Permissions = permissions.ToJson();

            _permissionGroupRepository.Update(permissionGroup);
            await _permissionGroupRepository.UnitOfWork.SaveChangesAsync();
            return await ToPermissionGroupViewModel(permissionGroup);
        }
    }
}
