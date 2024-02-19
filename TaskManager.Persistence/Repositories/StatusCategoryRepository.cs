namespace TaskManager.Persistence.Repositories;

public class StatusCategoryRepository(IDbContext context) : GenericRepository<StatusCategory>(context)
    , IStatusCategoryRepository
{

    public async Task<IReadOnlyCollection<StatusCategory>> GetStatusCategorysAsync()
    {
        var statusCategories = await Entity
            .AsNoTracking()
            .ToListAsync();

        return statusCategories.AsReadOnly();
    }

    public async Task<StatusCategory?> GetDoneStatusCategoryAsync()
    {
        var doneStatusCategory = await Entity
            .AsNoTracking()
            .Where(sc => sc.Code == StatusCategoryConstants.DoneCode)
            .FirstOrDefaultAsync();

        return doneStatusCategory;
    }

    public async Task<IReadOnlyCollection<StatusCategory>> GetStatusCategorysForStatusOfIssueAsync()
    {
        var statusCategoryCodes = new List<string>()
        {
            StatusCategoryConstants.ToDoCode,
            StatusCategoryConstants.InProgressCode,
            StatusCategoryConstants.DoneCode,
        };
        var statusCategories = await Entity
            .AsNoTracking()
            .Where(sc => statusCategoryCodes.Contains(sc.Code))
            .ToListAsync();

        return statusCategories.AsReadOnly();
    }
}
