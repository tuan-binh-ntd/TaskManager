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
            if (await appDbContext.IssueEvents.AnyAsync()) return;
            var issueEvemts = new List<IssueEvent>()
            {
                new IssueEvent()
                {
                    Name = "Issue Created (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Updated (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Assigned (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Resolved (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Closed (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Commented (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Comment Edited (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Comment Deleted (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Reopened (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Deleted (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Moved (System)"
                },
                new IssueEvent()
                {
                    Name = "Work Logged On Issue (System)"
                },
                new IssueEvent()
                {
                    Name = "Work Started On Issue (System)"
                },
                new IssueEvent()
                {
                    Name = "Work Stopped On Issue (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Worklog Updated (System)"
                },
                new IssueEvent()
                {
                    Name = "Issue Worklog Deleted (System)"
                },
                new IssueEvent()
                {
                    Name = "Generic Event (System)"
                },
            };

            appDbContext.IssueEvents.AddRange(issueEvemts);
            await appDbContext.SaveChangesAsync();

            if (await appDbContext.Priorities.AnyAsync()) return;
            var priorities = new List<Priority>()
            {
                new Priority()
                {
                    Name = CoreConstants.LowestName,
                    Description = "Trivial problem with little or no impact on progress.",
                    Color = "#999999"
                },
                new Priority()
                {
                    Name = CoreConstants.LowName,
                    Description = "Minor problem or easily worked around.",
                    Color = "#707070"
                },
                new Priority()
                {
                    Name = CoreConstants.MediumName,
                    Description = "Has the potential to affect progress.",
                    Color = "#f79232"
                },
                new Priority()
                {
                    Name = CoreConstants.HighName,
                    Description = "Serious problem that could block progress.",
                    Color = "#f15C75"
                },
                new Priority()
                {
                    Name = CoreConstants.HighestName,
                    Description = "This problem will block progress.",
                    Color = "#d04437"
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
                },
            };

            await appDbContext.StatusCategories.AddRangeAsync(statusCategories);
            await appDbContext.SaveChangesAsync();

            var versionStatusCategory = await appDbContext.StatusCategories.Where(e => e.Code == CoreConstants.VersionCode).FirstOrDefaultAsync();

            if (versionStatusCategory is not null) return;
            appDbContext.StatusCategories.Add(new StatusCategory()
            {
                Name = "Version status",
                Color = "#26282A",
                Code = CoreConstants.VersionCode
            });

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
