namespace TaskManager.Persistence.Repositories;

public class LabelRepository(IDbContext context) : GenericRepository<Label>(context)
    , ILabelRepository
{

    public async Task<IReadOnlyCollection<Label>> GetLabelsByProjectIdAsync(Guid projectId)
    {
        var labels = await Entity
            .Where(l => l.ProjectId == projectId)
            .ToListAsync();

        return labels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<LabelViewModel>> GetLabelViewModelsByIssueIdAsync(Guid issueId)
    {
        var labels = await (from li in DbContext.Set<LabelIssue>().Where(li => li.IssueId == issueId)
                            join l in Entity on li.LabelId equals l.Id
                            select new LabelViewModel
                            {
                                Id = l.Id,
                                Name = l.Name,
                                Color = l.Color,
                                Description = l.Description,
                            })
                            .ToListAsync();

        return labels.AsReadOnly();
    }

    public async Task<PaginationResult<LabelViewModel>> GetLabelViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput)
    {
        var labels = await (from l in Entity.Where(l => l.ProjectId == projectId)
                            select new LabelViewModel
                            {
                                Id = l.Id,
                                Name = l.Name,
                                Color = l.Color,
                                Description = l.Description,
                            })
                            .Pagination(paginationInput);
        return labels;
    }
}
