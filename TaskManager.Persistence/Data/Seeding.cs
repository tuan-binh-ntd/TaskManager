namespace TaskManager.Persistence.Data;

public class Seeding
{
    public static async Task SeedUsers(
        AppDbContext appDbContext)
    {
        #region Seed Issue Event
        if (await appDbContext.IssueEvents.AnyAsync()) return;
        var issueEvemts = new List<IssueEvent>()
        {
            new()
            {
                Name = IssueEventConstants.IssueCreatedName
            },
            new()
            {
                Name = IssueEventConstants.IssueEditedName
            },
            new()
            {
                Name = IssueEventConstants.SomeoneAssignedToAIssueName
            },
            new()
            {
                Name = IssueEventConstants.IssueDeletedName
            },
            new()
            {
                Name = IssueEventConstants.IssueMovedName
            },
            new()
            {
                Name = IssueEventConstants.SomeoneMadeACommentName
            },
            new()
            {
                Name = IssueEventConstants.CommentEditedName
            },
            new()
            {
                Name = IssueEventConstants.CommentDeletedName
            },
            new()
            {
                Name = IssueEventConstants.SomeoneMadeAnAttachmentName
            },
            new()
            {
                Name = IssueEventConstants.AttachmentDeletedName
            },
        };

        appDbContext.IssueEvents.AddRange(issueEvemts);
        await appDbContext.SaveChangesAsync();
        #endregion

        #region Seed Criteria
        if (await appDbContext.Criterias.AnyAsync()) return;
        var criterias = new List<Criteria>
        {
            new() {
                Name = CriteriaConstants.ProjectCriteriaName,
            },
            new() {
                Name = CriteriaConstants.TypeCriteriaName,
            },
            new() {
                Name = CriteriaConstants.StatusCriteriaName,
            },
            new() {
                Name = CriteriaConstants.AssigneeCriteriaName,
            },
            new() {
                Name = CriteriaConstants.CreatedCriteriaName,
            },
            new() {
                Name = CriteriaConstants.DueDateCriteriaName,
            },
            new() {
                Name = CriteriaConstants.FixVersionsCriteriaName,
            },
            new() {
                Name = CriteriaConstants.LabelsCriteriaName,
            },
            new() {
                Name = CriteriaConstants.PriorityCriteriaName,
            },
            new() {
                Name = CriteriaConstants.ReporterCriteriaName,
            },
            new() {
                Name = CriteriaConstants.ResolutionCriteriaName,
            },
            new() {
                Name = CriteriaConstants.ResolvedCriteriaName,
            },
            new() {
                Name = CriteriaConstants.SprintCriteriaName,
            },
            new() {
                Name = CriteriaConstants.StatusCategoryCriteriaName,
            },
            new() {
                Name = CriteriaConstants.UpdatedCriteriaName,
            },
        };
        appDbContext.Criterias.AddRange(criterias);
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
                Code = StatusCategoryConstants.ToDoCode
            },
            new()
            {
                Name = "In-progress status",
                Color = "#45b6fe",
                Code = StatusCategoryConstants.InProgressCode
            },
            new()
            {
                Name = "Done status",
                Color = "#b4d3b2",
                Code = StatusCategoryConstants.DoneCode
            },
            new()
            {
                Name ="Hide status",
                Color = "#26282A",
                Code = StatusCategoryConstants.HideCode
            },
            new()
            {
                Name = "Version status",
                Color = "#26282A",
                Code = StatusCategoryConstants.VersionCode
            }
        };

        await appDbContext.StatusCategories.AddRangeAsync(statusCategories);
        await appDbContext.SaveChangesAsync();
        #endregion
    }
}
