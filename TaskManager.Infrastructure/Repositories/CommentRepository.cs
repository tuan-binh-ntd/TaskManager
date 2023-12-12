using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public CommentViewModel Add(Comment comment)
    {
        return _context.Add(comment).Entity.Adapt<CommentViewModel>();
    }

    public void Delete(Comment comment)
    {
        _context.Comments.Remove(comment);
    }

    public async Task<IReadOnlyCollection<CommentViewModel>> Gets()
    {
        var comments = await _context.Comments.AsNoTracking().ProjectToType<CommentViewModel>().ToListAsync();
        return comments.AsReadOnly();
    }

    public void Update(Comment comment)
    {
        _context.Entry(comment).State = EntityState.Modified;
    }

    public async Task<IReadOnlyCollection<Comment>> GetByIssueId(Guid issueId)
    {
        var comments = await _context.Comments.AsNoTracking().Where(c => c.IssueId == issueId).OrderBy(c => c.CreationTime).ToListAsync();
        return comments.AsReadOnly();
    }

    public async Task<Comment?> GetById(Guid id)
    {
        var comment = await _context.Comments.Where(c => c.Id == id).FirstOrDefaultAsync();
        return comment;
    }
}
