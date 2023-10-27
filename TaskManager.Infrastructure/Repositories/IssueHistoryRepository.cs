using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class IssueHistoryRepository : IIssueHistoryRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public IssueHistoryRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IssueHistoryViewModel Add(IssueHistory issueHistory)
        {
            return _context.IssueHistories.Add(issueHistory).Entity.Adapt<IssueHistoryViewModel>();
        }

        public void Delete(Guid id)
        {
            var issueHistory = _context.IssueHistories.SingleOrDefault(e => e.Id == id);
            _context.IssueHistories.Remove(issueHistory!);
        }

        public async Task<IReadOnlyCollection<IssueHistoryViewModel>> Gets()
        {
            var issueHistories = await _context.IssueHistories.Select(
                e => new IssueHistoryViewModel
                {
                    Id = e.Id,
                    CreatorUserId = e.CreatorUserId,
                    CreationTime = e.CreationTime,
                    Content = e.Content
                }).ToListAsync();
            return issueHistories.AsReadOnly();
        }

        public void Update(IssueHistory issueHistory)
        {
            _context.Entry(issueHistory).State = EntityState.Modified;
        }

        public async Task<IReadOnlyCollection<IssueHistory>> GetByIssueId(Guid issueId)
        {
            var issueHistories = await _context.IssueHistories.Where(ih => ih.IssueId == issueId).ToListAsync();
            return issueHistories.AsReadOnly();
        }
    }
}
