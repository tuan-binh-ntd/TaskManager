using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            if (await appDbContext.IssueTypes.AnyAsync()) return;

            var issueTypes = new List<IssueType>()
            {
                new IssueType()
                {
                    Name = "Epic",
                    Description = "Epics track collections of related bugs, stories, and tasks."
                },
                new IssueType()
                {
                    Name = "Bug",
                    Description = "Bugs track problems or errors."
                },
                new IssueType()
                {
                    Name = "Story",
                    Description = "Stories track functionality or features expressed as user goals."
                },
                new IssueType()
                {
                    Name = "Task",
                    Description = "Tasks track small, distinct pieces of work."
                },
                new IssueType()
                {
                    Name = "Subtask",
                    Description = "Subtasks track small pieces of work that are part of a larger task."
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
