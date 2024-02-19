namespace TaskManager.Persistence.Repositories;

public class CommentRepository(IDbContext context) : GenericRepository<Comment>(context)
    , ICommentRepository
{
    public async Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueIdAsync(Guid issueId)
    {
        var comments = await Entity
            .AsNoTracking()
            .Where(c => c.IssueId == issueId)
            .Select(c => new CommentViewModel
            {
                Id = c.Id,
                Content = c.Content,
                CreationTime = c.CreationTime,
                CreatorUserId = c.CreatorUserId,
                IsEdited = c.IsEdited,
            })
            .OrderBy(c => c.CreationTime)
            .ToListAsync();

        return comments.AsReadOnly();
    }
}
