using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.ViewModel
{
    public class UserViewModel : BaseDto<AppUser, UserViewModel>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public override void AddCustomMappings()
        {
            // Mapster can map properties with different names
            // Here we split the price into two properties for the model behind the DTO
            SetCustomMappings()
                .Map(dest => dest.Role, src => src.UserRoles!.Where(ur => ur.UserId == src.Id).Select(ur => ur.Role).FirstOrDefault());
        }
    }
}
