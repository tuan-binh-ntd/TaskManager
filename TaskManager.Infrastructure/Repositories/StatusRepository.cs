using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public StatusRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Status Add(Status status)
    {
        return _context.Statuses.Add(status).Entity;
    }

    public void Delete(Guid id)
    {
        var status = _context.Statuses.FirstOrDefault(x => x.Id == id);
        _context.Statuses.Remove(status!);
    }

    public void Update(Status status)
    {
        _context.Entry(status).State = EntityState.Modified;
    }

    public void AddRange(ICollection<Status> statuses)
    {
        _context.Statuses.AddRange(statuses);
    }

    public async Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelByProjectId(Guid projectId)
    {
        var statusCodes = new List<string>()
        {
            CoreConstants.ToDoCode,
            CoreConstants.InProgressCode,
            CoreConstants.DoneCode
        };

        var statuses = await (from sc in _context.StatusCategories.AsNoTracking().Where(sc => statusCodes.Contains(sc.Code))
                              join s in _context.Statuses.AsNoTracking().Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                              join i in _context.Issues.AsNoTracking() on s.Id equals i.StatusId into ij
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
        return statuses.AsReadOnly();
    }

    public async Task<Status> GetById(Guid id)
    {
        var status = await _context.Statuses.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
        return status!;
    }

    public async Task<Status> GetUnreleasedStatus(Guid projectId)
    {
        var status = await _context.Statuses.AsNoTracking().Where(e => e.Name == CoreConstants.UnreleasedStatusName && e.ProjectId == projectId).FirstOrDefaultAsync();
        return status!;
    }

    public async Task<PaginationResult<StatusViewModel>> GetByProjectIdPaging(Guid projectId, PaginationInput paginationInput)
    {
        var statusCodes = new List<string>()
        {
            CoreConstants.ToDoCode,
            CoreConstants.InProgressCode,
            CoreConstants.DoneCode
        };

        var query = from sc in _context.StatusCategories.AsNoTracking().Where(sc => statusCodes.Contains(sc.Code))
                    join s in _context.Statuses.AsNoTracking().Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                    join i in _context.Issues.AsNoTracking() on s.Id equals i.StatusId into ij
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

        return await query.Pagination(paginationInput);
    }

    public async Task<string?> GetNameOfStatus(Guid statusId)
    {
        string? name = await _context.Statuses.AsNoTracking().Where(s => s.Id == statusId).Select(s => s.Name).FirstOrDefaultAsync();
        return name;
    }

    public async Task<bool> CheckStatusBelongDone(Guid statusId)
    {
        var query = from sc in _context.StatusCategories.Where(sc => sc.Code == CoreConstants.DoneCode)
                    join s in _context.Statuses.Where(s => s.Id == statusId) on sc.Id equals s.StatusCategoryId
                    select s;

        return await query.AnyAsync();
    }

    public async Task<IReadOnlyCollection<StatusViewModel>> GetStatusViewModelsAsync(Guid projectId)
    {
        var statusViewModels = await (from sc in _context.StatusCategories.Where(sc => sc.Code == CoreConstants.VersionCode)
                                      join s in _context.Statuses.Where(S => S.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                                      select new StatusViewModel
                                      {
                                          Id = s.Id,
                                          Name = s.Name,
                                          Description = s.Description,
                                          IsMain = s.IsMain,
                                          StatusCategoryId = s.StatusCategoryId,
                                      }).ToListAsync();

        return statusViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Status>> GetByProjectId(Guid projectId)
    {
        var statusCodes = new List<string>()
        {
            CoreConstants.ToDoCode,
            CoreConstants.InProgressCode,
            CoreConstants.DoneCode
        };

        var statuses = await (from sc in _context.StatusCategories.AsNoTracking().Where(sc => statusCodes.Contains(sc.Code))
                              join s in _context.Statuses.AsNoTracking().Where(s => s.ProjectId == projectId) on sc.Id equals s.StatusCategoryId
                              select s).ToListAsync();
        return statuses.AsReadOnly();
    }
}
