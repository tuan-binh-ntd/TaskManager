using Mapster;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Helper
{
    public class MappingRegistration : IRegister
    {
        void IRegister.Register(TypeAdapterConfig config)
        {
            config.NewConfig<AppUser, UserViewModel>();
            config.NewConfig<AppUser, SignUpDto>();

            config.NewConfig<AppRole, AppRoleDto>();
            config.NewConfig<AppRole, RoleViewModel>();
            config.NewConfig<List<AppRole>, List<AppRoleDto>>();

            config.NewConfig<Project, ProjectDto>();
            config.NewConfig<Project, ProjectViewModel>();
            config.NewConfig<List<Project>, List<ProjectViewModel>>();
        }
    }
}
