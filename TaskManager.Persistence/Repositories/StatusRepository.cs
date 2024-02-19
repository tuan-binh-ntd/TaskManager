namespace TaskManager.Persistence.Repositories;

public class StatusRepository(IDbContext context) : GenericRepository<Status>(context)
    , IStatusRepository
{
    public async Task<Status> GetUnreleasedStatusByProjectIdAsync(Guid projectId)
    {
        var status = await Entity
            .AsNoTracking()
            .Where(e => e.Name == StatusConstants.UnreleasedStatusName && e.ProjectId == projectId)
            .FirstOrDefaultAsync();

        return status!;
    }

    public async Task<PaginationResult<StatusViewModel>> GetStatusViewModelsByProjectIdPagingAsync(Guid projectId, PaginationInput paginationInput)
    {
        var statusCodes = new List<string>()
        {
            StatusCategoryConstants.ToDoCode,
            StatusCategoryConstants.InProgressCode,
            StatusCategoryConstants.DoneCode
        };

#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var query = from sc in DbContext.Set<StatusCategory>().AsNoTracking().Where(sc => statusCodes.Contains(sc.Code))
                    join s in Entity.AsNoTracking().Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                    join i in DbContext.Set<Issue>().AsNoTracking() on s.Id equals i.StatusId into ij
                    from ilj in ij.DefaultIfEmpty()
                    group new { s, ilj } by new { s.Id, s.Name, s.IsMain, s.Description, s.StatusCategoryId } into g
                    select new StatusViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Description = g.Key.Description,
                        IsMain = g.Key.IsMain,
                        StatusCategoryId = g.Key.StatusCategoryId,
                        IssueCount = g.Count(g => g.ilj.Id != null)
                    };
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'

        return await query.Pagination(paginationInput);
    }

    public async Task<string?> GetNameOfStatusAsync(Guid statusId)
    {
        string? name = await Entity
            .AsNoTracking()
            .Where(s => s.Id == statusId)
            .Select(s => s.Name)
            .FirstOrDefaultAsync();

        return name;
    }

    public async Task<bool> CheckStatusBelongDoneAsync(Guid statusId)
    {
        var query = from sc in DbContext.Set<StatusCategory>().Where(sc => sc.Code == StatusCategoryConstants.DoneCode)
                    join s in Entity.Where(s => s.Id == statusId) on sc.Id equals s.StatusCategoryId
                    select s;

        return await query.AnyAsync();
    }

    public async Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsByProjectIdAsync(Guid projectId)
    {
        var statusCodes = new List<string>()
        {
            StatusCategoryConstants.ToDoCode,
            StatusCategoryConstants.InProgressCode,
            StatusCategoryConstants.DoneCode
        };

#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        var statuses = await (from sc in DbContext.Set<StatusCategory>().AsNoTracking().Where(sc => statusCodes.Contains(sc.Code))
                              join s in Entity.AsNoTracking().Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                              join i in DbContext.Set<Issue>().AsNoTracking() on s.Id equals i.StatusId into ij
                              from ilj in ij.DefaultIfEmpty()
                              group new { s, ilj } by new { s.Id, s.Name, s.IsMain, s.Description, s.StatusCategoryId } into g
                              select new StatusViewModel
                              {
                                  Id = g.Key.Id,
                                  Name = g.Key.Name,
                                  Description = g.Key.Description,
                                  IsMain = g.Key.IsMain,
                                  StatusCategoryId = g.Key.StatusCategoryId,
                                  IssueCount = g.Count(g => g.ilj.Id != null)
                              }).ToListAsync();
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        return statuses.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsOfIssueByProjectIdAsync(Guid projectId)
    {
        var statusCodes = new List<string>()
        {
            StatusCategoryConstants.ToDoCode,
            StatusCategoryConstants.InProgressCode,
            StatusCategoryConstants.DoneCode
        };

        var statuses = await (from sc in DbContext.Set<StatusCategory>().AsNoTracking().Where(sc => sc.Code == StatusCategoryConstants.VersionCode)
                              join s in Entity.AsNoTracking().Where(S => S.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                              select new StatusViewModel
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  Description = s.Description,
                                  IsMain = s.IsMain,
                                  StatusCategoryId = s.StatusCategoryId,
                              }).ToListAsync();

        return statuses.AsReadOnly();
    }

    public async Task<bool> IsReleaseStatusAsync(Guid statusId)
    {
        return await Entity
            .Where(s => s.Id == statusId && s.Name == StatusConstants.ReleasedStatusName)
            .AnyAsync();
    }

    public async Task<IReadOnlyCollection<Status>> GetStatusesByProjectIdAsync(Guid projectId)
    {
        var statusCodes = new List<string>()
        {
            StatusCategoryConstants.ToDoCode,
            StatusCategoryConstants.InProgressCode,
            StatusCategoryConstants.DoneCode
        };

        var statuses = await (from sc in DbContext.Set<StatusCategory>().AsNoTracking().Where(sc => statusCodes.Contains(sc.Code))
                              join s in Entity.AsNoTracking().Where(S => S.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                              select s).ToListAsync();

        return statuses.AsReadOnly();
    }
}
