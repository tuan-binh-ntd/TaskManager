using AutoMapper;
using TaskManager.Core.Entities;
using TaskManager.Infrastructure.DTOs;
using TaskManager.Infrastructure.ViewModel;

namespace TaskManager.Infrastructure.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserViewModel>().ReverseMap();
            CreateMap<AppUser, SignUpDto>().ReverseMap();

            CreateMap<AppRole, AppRoleDto>().ReverseMap();

            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectViewModel, Project>().ReverseMap();
        }
    }
}
