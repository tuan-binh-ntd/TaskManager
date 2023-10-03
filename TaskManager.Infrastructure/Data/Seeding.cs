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

                },
                new IssueType()
                {
                    Name = "Bug"
                },
                new IssueType()
                {
                    Name = "Story"
                },
                new IssueType()
                {
                    Name = "Task"
                },
                new IssueType()
                {
                    Name = "Subtask"
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
