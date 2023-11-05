using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class IssueTypeRepository : IIssueTypeRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public IssueTypeRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IssueTypeViewModel Add(IssueType issueType)
        {
            return _context.IssueTypes.Add(issueType).Entity.Adapt<IssueTypeViewModel>();
        }

        public void Delete(Guid id)
        {
            var issueType = _context.IssueTypes.SingleOrDefault(e => e.Id == id);
            _context.IssueTypes.Remove(issueType!);
        }

        public async Task<IReadOnlyCollection<IssueTypeViewModel>> Gets()
        {
            var issueTypes = await _context.IssueTypes.AsNoTracking().Select(e => new IssueTypeViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Icon = e.Icon,
                Level = e.Level,
            }).ToListAsync();
            return issueTypes.AsReadOnly();
        }

        public void Update(IssueType issueType)
        {
            _context.Entry(issueType).State = EntityState.Modified;
        }

        public async Task<IReadOnlyCollection<IssueTypeViewModel>> GetsByProjectId(Guid projectId)
        {
            var issueTypes = await _context.IssueTypes.AsNoTracking()
                .Where(e => e.ProjectId == projectId || e.ProjectId == null).Select(e => new IssueTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Icon = e.Icon,
                    Level = e.Level,
                }).ToListAsync();
            return issueTypes.AsReadOnly();
        }

        public async Task<IssueType> Get(Guid id)
        {
            var issueType = await _context.IssueTypes.AsNoTracking().Where(e => e.Id == id).SingleOrDefaultAsync();
            return issueType!;
        }

        public async Task<IssueType> GetSubtask()
        {
            var issueType = await _context.IssueTypes.AsNoTracking().
               Where(e => e.Name == CoreConstants.SubTaskName).SingleOrDefaultAsync();
            return issueType!;
        }

        public async Task<IssueType> GetEpic()
        {
            var issueType = await _context.IssueTypes.AsNoTracking().
               Where(e => e.Name == CoreConstants.EpicName).SingleOrDefaultAsync();
            return issueType!;
        }

        public async Task<PaginationResult<IssueTypeViewModel>> GetsByProjectIdPaging(Guid projectId, PaginationInput paginationInput)
        {
            var query = _context.IssueTypes.AsNoTracking()
                .Where(e => e.ProjectId == projectId || e.ProjectId == null).Select(e => new IssueTypeViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Icon = e.Icon,
                    Level = e.Level,
                });

            return await query.Pagination(paginationInput);
        }
    }
}
