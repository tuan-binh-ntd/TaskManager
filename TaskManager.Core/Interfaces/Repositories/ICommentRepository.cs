namespace TaskManager.Core.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueIdAsync(Guid issueId);
    void Insert(Comment comment);
    void Remove(Comment comment);
    Task<Comment?> GetByIdAsync(Guid id);
    void Update(Comment comment);
}
