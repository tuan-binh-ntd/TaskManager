using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data
{
    public class Seeding
    {
        public static async Task SeedUsers(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            AppDbContext appDbContext)
        {
            if (await appDbContext.Priorities.AnyAsync()) return;
            var priorities = new List<Priority>()
            {
                new Priority()
                {
                    Name = CoreConstants.NormalName,
                },
                new Priority()
                {
                    Name = CoreConstants.MediumName,
                },
                new Priority()
                {
                    Name = CoreConstants.HighName,
                },
                new Priority()
                {
                    Name = CoreConstants.UrgentName,
                }
            };

            appDbContext.Priorities.AddRange(priorities);
            await appDbContext.SaveChangesAsync();


            if (await appDbContext.StatusCategories.AnyAsync()) return;
            var statusCategories = new List<StatusCategory>
            {
                new StatusCategory()
                {
                    Name = "To-do status",
                    Color = "#dddddd",
                    Code = CoreConstants.ToDoCode
                },
                new StatusCategory()
                {
                    Name = "In-progress status",
                    Color = "#45b6fe",
                    Code = CoreConstants.InProgressCode
                },
                new StatusCategory()
                {
                    Name = "Done status",
                    Color = "#b4d3b2",
                    Code = CoreConstants.DoneCode
                },
                new StatusCategory()
                {
                    Name ="Hide status",
                    Color = "#26282A",
                    Code = CoreConstants.HideCode
                }
            };

            await appDbContext.StatusCategories.AddRangeAsync(statusCategories);
            await appDbContext.SaveChangesAsync();

            if (await appDbContext.IssueTypes.AnyAsync()) return;
            var issueTypes = new List<IssueType>()
            {
                new IssueType()
                {
                    Name = CoreConstants.EpicName,
                    Description = "Epics track collections of related bugs, stories, and tasks.",
                    Level = 1,
                },
                new IssueType()
                {
                    Name = CoreConstants.BugName,
                    Description = "Bugs track problems or errors.",
                    Level = 2,
                },
                new IssueType()
                {
                    Name = CoreConstants.StoryName,
                    Description = "Stories track functionality or features expressed as user goals.",
                    Level = 2,
                },
                new IssueType()
                {
                    Name = CoreConstants.TaskName,
                    Description = "Tasks track small, distinct pieces of work.",
                    Level = 2,
                },
                new IssueType()
                {
                    Name = CoreConstants.SubTaskName,
                    Description = "Subtasks track small pieces of work that are part of a larger task.",
                    Level = 3,
                }
            };

            await appDbContext.IssueTypes.AddRangeAsync(issueTypes);
            await appDbContext.SaveChangesAsync();

            if (await userManager.Users.AnyAsync()) return;

            // Create role of system
            var roles = new List<AppRole>
            {
                new AppRole{ Name = "Admin" },
                new AppRole{ Name = "Employee" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            // Create Addmin Account
            AppUser admin = new()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
            };

            await userManager.CreateAsync(admin, "Abcd1234!");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Employee" });
        }
    }
}
