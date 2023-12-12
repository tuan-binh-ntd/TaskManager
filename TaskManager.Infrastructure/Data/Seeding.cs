using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data
{
    public class Seeding
    {
        public static async Task SeedUsers(
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
                new() {
                    Name = CoreConstants.ProjectCriteriaName,
                },
                new() {
                    Name = CoreConstants.TypeCriteriaName,
                },
                new() {
                    Name = CoreConstants.StatusCriteriaName,
                },
                new() {
                    Name = CoreConstants.AssigneeCriteriaName,
                },
                new() {
                    Name = CoreConstants.CreatedCriteriaName,
                },
                new() {
                    Name = CoreConstants.DueDateCriteriaName,
                },
                new() {
                    Name = CoreConstants.FixVersionsCriteriaName,
                },
                new() {
                    Name = CoreConstants.LabelsCriteriaName,
                },
                new() {
                    Name = CoreConstants.PriorityCriteriaName,
                },
                new() {
                    Name = CoreConstants.ReporterCriteriaName,
                },
                new() {
                    Name = CoreConstants.ResolutionCriteriaName,
                },
                new() {
                    Name = CoreConstants.ResolvedCriteriaName,
                },
                new() {
                    Name = CoreConstants.SprintCriteriaName,
                },
                new() {
                    Name = CoreConstants.StatusCategoryCriteriaName,
                },
                new() {
                    Name = CoreConstants.UpdatedCriteriaName,
                },
            };
            appDbContext.Criterias.AddRange(criterias);
            await appDbContext.SaveChangesAsync();
            #endregion

            #region Seed Issue Event
            if (await appDbContext.IssueEvents.AnyAsync()) return;
            var issueEvemts = new List<IssueEvent>()
            {
                new()
                {
                    Name = CoreConstants.IssueCreatedName
                },
                new()
                {
                    Name = CoreConstants.IssueUpdatedName
                },
                new()
                {
                    Name = CoreConstants.IssueAssignedName
                },
                new()
                {
                    Name = CoreConstants.IssueResolvedName
                },
                new()
                {
                    Name = CoreConstants.IssueClosedName
                },
                new()
                {
                    Name = CoreConstants.IssueCommentedName
                },
                new()
                {
                    Name = CoreConstants.IssueCommentEditedName
                },
                new()
                {
                    Name = CoreConstants.IssueCommentDeletedName
                },
                new()
                {
                    Name = CoreConstants.IssueReopenedName
                },
                new()
                {
                    Name = CoreConstants.IssueDeletedName
                },
                new()
                {
                    Name = CoreConstants.IssueMovedName
                },
                new()
                {
                    Name = CoreConstants.WorkLoggedOnIssueName
                },
                new()
                {
                    Name = CoreConstants.WorkStartedOnIssueName
                },
                new()
                {
                    Name = CoreConstants.WorkStoppedOnIssueName
                },
                new()
                {
                    Name = CoreConstants.IssueWorklogUpdatedName
                },
                new()
                {
                    Name = CoreConstants.IssueWorklogDeletedName
                },
                new()
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
                new()
                {
                    Name = "To-do status",
                    Color = "#dddddd",
                    Code = CoreConstants.ToDoCode
                },
                new()
                {
                    Name = "In-progress status",
                    Color = "#45b6fe",
                    Code = CoreConstants.InProgressCode
                },
                new()
                {
                    Name = "Done status",
                    Color = "#b4d3b2",
                    Code = CoreConstants.DoneCode
                },
                new()
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

        }
    }
}
