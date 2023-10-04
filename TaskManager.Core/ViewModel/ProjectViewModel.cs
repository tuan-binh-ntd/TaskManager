using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.ViewModel
{
    public class ProjectViewModel : BaseDto<ProjectViewModel, Project>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsFavourite { get; set; } = false;
        public UserViewModel? Leader { get; set; }
        public ICollection<UserViewModel>? Members { get; set; }

        public override void AddCustomMappings()
        {
            // Mapster can map properties with different names
            // Here we split the price into two properties for the model behind the DTO
            SetCustomMappingsReverse()
                .Map(dest => dest.Leader, src => src.UserProjects!.Where(up => up.Role == CoreConstants.LeaderRole).Select(up => up.User).FirstOrDefault())
                .Map(dest => dest.Members, src => src.UserProjects!.Where(up => up.Role != CoreConstants.LeaderRole).Select(up => up.User).ToList());
        }
    }
}
