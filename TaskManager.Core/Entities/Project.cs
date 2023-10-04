using Mapster;
using TaskManager.Core.Core;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsFavourite { get; set; } = false;

        //Relationship
        public Backlog? Backlog { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; }

        public ProjectViewModel ToViewModel()
        {
            var viewModel = this.Adapt<ProjectViewModel>();
            if (UserProjects is not null && UserProjects.Any())
            {
                viewModel.Leader = UserProjects.Where(up => up.Role == CoreConstants.LeaderRole)
                    .Select(up => new UserViewModel()
                    {
                        Id = up.User!.Id,
                        Name = up.User!.Name,
                        Department = up.User!.Department,
                        Organization = up.User!.Organization,
                        AvatarUrl = up.User!.AvatarUrl,
                        JobTitle = up.User!.JobTitle,
                        Location = up.User!.Location,
                        Email = up.User!.Email,
                        Role = up.Role
                    })
                    .SingleOrDefault();

                viewModel.Members = UserProjects.Where(up => up.Role != CoreConstants.LeaderRole)
                    .Select(up => new UserViewModel()
                    {
                        Id = up.User!.Id,
                        Name = up.User!.Name,
                        Department = up.User!.Department,
                        Organization = up.User!.Organization,
                        AvatarUrl = up.User!.AvatarUrl,
                        JobTitle = up.User!.JobTitle,
                        Location = up.User!.Location,
                        Email = up.User!.Email,
                        Role = up.Role
                    })
                    .ToList();
            }

            return viewModel;
        }
    }
}
