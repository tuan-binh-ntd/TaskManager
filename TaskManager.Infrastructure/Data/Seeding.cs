using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;

namespace TaskManager.Infrastructure.Data
{
    public class Seeding
    {
        public static async Task SeedUsers(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            AppDbContext appDbContext)
        {
            #region Permission
            if (await appDbContext.Permissions.AnyAsync()) return;

            var timelinePermission = new Permission()
            {
                Name = CoreConstants.TimelinePermissionName,
            };

            var backlogPermission = new Permission()
            {
                Name = CoreConstants.BacklogPermissionName,
            };

            var boardPermission = new Permission()
            {
                Name = CoreConstants.BoardPermissionName,
            };

            var codePermission = new Permission()
            {
                Name = CoreConstants.CodePermissionName,
            };

            var detailsPermission = new Permission()
            {
                Name = CoreConstants.DetailsPermissionName,
            };

            var issueTypesPermission = new Permission()
            {
                Name = CoreConstants.IssueTypesPermissionName,
            };

            var prioritiesPermission = new Permission()
            {
                Name = CoreConstants.PrioritiesPermissionName,
            };

            var statusesPermission = new Permission()
            {
                Name = CoreConstants.StatusesPermissionName,
            };

            var notificationsPermission = new Permission()
            {
                Name = CoreConstants.NotificationsPermissionName,
            };

            var accessPermission = new Permission()
            {
                Name = CoreConstants.AccessPermissionName,
            };

            var featuresPermission = new Permission()
            {
                Name = CoreConstants.FeaturesPermissionName,
            };

            var projectPermission = new Permission()
            {
                Name = CoreConstants.ProjectPermissionName,
            };

            var parentPermissions = new List<Permission>()
            {
                timelinePermission, backlogPermission, boardPermission, codePermission, detailsPermission, issueTypesPermission, prioritiesPermission, statusesPermission, notificationsPermission, accessPermission, featuresPermission, projectPermission
            };

            appDbContext.Permissions.AddRange(parentPermissions);
            await appDbContext.SaveChangesAsync();
            #endregion

            #region Seed Criteria
            if (await appDbContext.Criterias.AnyAsync()) return;
            var criterias = new List<Criteria>
            {
                new Criteria
                {
                    Name = CoreConstants.ProjectCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.TypeCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.StatusCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.AssigneeCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.CreatedCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.DueDateCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.FixVersionsCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.LabelsCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.PriorityCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.ReporterCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.ResolutionCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.ResolvedCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.SprintCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.StatusCategoryCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.SummaryCriteriaName,
                },
                new Criteria
                {
                    Name = CoreConstants.UpdatedCriteriaName,
                },
            };
            appDbContext.Criterias.AddRange(criterias);
            await appDbContext.SaveChangesAsync();
            #endregion

            #region Seed Default Filters
            if (await appDbContext.Filters.AnyAsync()) return;

            //var criterias = await appDbContext.Criterias.ToListAsync();

            var assigneeCriteria = criterias.Where(c => c.Name == CoreConstants.AssigneeCriteriaName).FirstOrDefault();
            var resolutionCriteria = criterias.Where(c => c.Name == CoreConstants.ResolutionCriteriaName).FirstOrDefault();
            var reporterCriteria = criterias.Where(c => c.Name == CoreConstants.ReporterCriteriaName).FirstOrDefault();
            var statusCategoryCriteria = criterias.Where(c => c.Name == CoreConstants.StatusCategoryCriteriaName).FirstOrDefault();
            var createdCriteria = criterias.Where(c => c.Name == CoreConstants.CreatedCriteriaName).FirstOrDefault();
            var resolvedCriteria = criterias.Where(c => c.Name == CoreConstants.ResolvedCriteriaName).FirstOrDefault();
            var updatedCriteria = criterias.Where(c => c.Name == CoreConstants.UpdatedCriteriaName).FirstOrDefault();

            #region Create My open issues filter
            var myOpenIssues = new Filter()
            {
                Name = CoreConstants.MyOpenIssuesFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            myOpenIssues.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = myOpenIssues.Id,
                    CriteriaId = assigneeCriteria!.Id,
                    Configuration = new AssigneeCriteria()
                    {
                        CurrentUser = true,
                    }.ToJson(),
                },
                new FilterCriteria()
                {
                    FilterId = myOpenIssues.Id,
                    CriteriaId = resolutionCriteria!.Id,
                    Configuration = new ResolutionCriteria()
                    {
                        Unresolved = true,
                        Done = false,
                    }.ToJson(),
                }
            };
            #endregion

            #region Create Reported by me filter
            var reportedByMe = new Filter()
            {
                Name = CoreConstants.ReportedByMeFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            reportedByMe.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = reportedByMe.Id,
                    CriteriaId = reporterCriteria!.Id,
                    Configuration = new ReporterCriteria()
                    {
                        CurrentUser = true
                    }.ToJson(),
                }
            };
            #endregion

            #region Create All issues filter
            var allIssues = new Filter()
            {
                Name = CoreConstants.AllIssuesFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };
            #endregion

            #region Create open issues
            var openIssues = new Filter()
            {
                Name = CoreConstants.OpenIssuesFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            openIssues.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = openIssues.Id,
                    CriteriaId = resolutionCriteria!.Id,
                    Configuration = new ResolutionCriteria()
                    {
                        Unresolved = true,
                        Done = false,
                    }.ToJson(),
                }
            };
            #endregion

            #region Create done issues
            var doneIssues = new Filter()
            {
                Name = CoreConstants.DoneIssuesFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            doneIssues.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = doneIssues.Id,
                    CriteriaId = statusCategoryCriteria!.Id,
                    Configuration = new StatusCategoryCriteria()
                    {
                        Todo = false,
                        InProgress = false,
                        Done = true,
                    }.ToJson(),
                }
            };
            #endregion

            #region Create Created recently filter
            var createdRecently = new Filter()
            {
                Name = CoreConstants.CreatedRecentlyFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            createdRecently.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = createdRecently.Id,
                    CriteriaId = createdCriteria!.Id,
                    Configuration = new CreatedCriteria()
                    {
                        WithinTheLast = new WithinTheLast(1, CoreConstants.WeekUnit)
                    }.ToJson(),
                }
            };
            #endregion

            #region Create resolved recently filter
            var resolvedRecently = new Filter()
            {
                Name = CoreConstants.ResolvedRecentlyFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            resolvedRecently.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = resolvedRecently.Id,
                    CriteriaId = resolvedCriteria!.Id,
                    Configuration = new ResolvedCriteria()
                    {
                        WithinTheLast = new WithinTheLast(1, CoreConstants.WeekUnit)
                    }.ToJson(),
                }
            };
            #endregion

            #region Create updated recently filter
            var updatedRecently = new Filter()
            {
                Name = CoreConstants.UpdatedRecentlyFilterName,
                Type = CoreConstants.DefaultFiltersType,
            };

            updatedRecently.FilterCriterias = new List<FilterCriteria>()
            {
                new FilterCriteria()
                {
                    FilterId = updatedRecently.Id,
                    CriteriaId = updatedCriteria!.Id,
                    Configuration = new UpdatedCriteria()
                    {
                        WithinTheLast = new WithinTheLast(1, CoreConstants.WeekUnit)
                    }.ToJson(),
                }
            };
            #endregion

            var filters = new List<Filter>
            {
                myOpenIssues, reportedByMe, allIssues, openIssues, doneIssues, createdRecently, resolvedRecently,updatedRecently
            };

            appDbContext.Filters.AddRange(filters);
            await appDbContext.SaveChangesAsync();
            #endregion

            #region Seed Issue Event
            if (await appDbContext.IssueEvents.AnyAsync()) return;
            var issueEvemts = new List<IssueEvent>()
            {
                new IssueEvent()
                {
                    Name = CoreConstants.IssueCreatedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueUpdatedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueAssignedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueResolvedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueClosedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueCommentedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueCommentEditedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueCommentDeletedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueReopenedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueDeletedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueMovedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.WorkLoggedOnIssueName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.WorkStartedOnIssueName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.WorkStoppedOnIssueName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueWorklogUpdatedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.IssueWorklogDeletedName
                },
                new IssueEvent()
                {
                    Name = CoreConstants.GenericEventName
                },
            };

            appDbContext.IssueEvents.AddRange(issueEvemts);
            await appDbContext.SaveChangesAsync();
            #endregion

            #region Seed StatusCategories
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
            #endregion

            #region Seed User
            if (await userManager.Users.AnyAsync()) return;

            // Create role of system
            var roles = new List<AppRole>
            {
                new AppRole{ Name = "Admin"},
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
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
            #endregion
        }
    }
}
