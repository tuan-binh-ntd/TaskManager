using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
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
        private async Task<PermissionGroupViewModel> ToPermissionGroupViewModel(PermissionGroup permissionGroup)
        {
            var permissionGroupViewModel = new PermissionGroupViewModel()
            {
                Id = permissionGroup.Id,
                Name = permissionGroup.Name,
                Permissions = permissionGroup.Permissions.FromJson<Permissions>()
            };

            return await Task.FromResult(permissionGroupViewModel);
        }

        private async Task<IReadOnlyCollection<PermissionGroupViewModel>> ToPermissionGroupViewModels(IReadOnlyCollection<PermissionGroup> permissionGroups)
        {
            var permissionGroupsViewModels = new List<PermissionGroupViewModel>();
            if (permissionGroups.Any())
            {

                foreach (var permissionGroup in permissionGroups)
                {
                    var permissionGroupViewModel = await ToPermissionGroupViewModel(permissionGroup);
                    permissionGroupsViewModels.Add(permissionGroupViewModel);
                }
                return permissionGroupsViewModels;
            }
            else
            {
                return permissionGroupsViewModels;
            }
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

        public async Task<IReadOnlyCollection<PermissionGroupViewModel>> GetPermissionGroupsByProjectId(Guid projectId)
        {
            var permissionGroups = await _permissionGroupRepository.GetByProjectId(projectId);
            return await ToPermissionGroupViewModels(permissionGroups);
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
