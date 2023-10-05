using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class IssueDetailRepository : IIssueDetailRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public IssueDetailRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IssueDetailViewModel Add(IssueDetail issueDetail)
        {
            return _context.IssueDetails.Add(issueDetail).Entity.Adapt<IssueDetailViewModel>();
        }

        public void Delete(Guid id)
        {
            var issueDetail = _context.IssueDetails.SingleOrDefault(e => e.Id == id);
            _context.IssueDetails.Remove(issueDetail!);
        }

        public async Task<IReadOnlyCollection<IssueDetailViewModel>> Gets()
        {
            var issueDetails = await _context.IssueDetails.ProjectToType<IssueDetailViewModel>().ToListAsync();
            return issueDetails.AsReadOnly();
        }

        public void Update(IssueDetail issueDetail)
        {
            _context.Entry(issueDetail).State = EntityState.Modified;
        }
    }
}
