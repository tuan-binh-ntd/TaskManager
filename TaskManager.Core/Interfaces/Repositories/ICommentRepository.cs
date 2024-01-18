namespace TaskManager.Core.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IReadOnlyCollection<CommentViewModel>> Gets();
    CommentViewModel Add(Comment comment);
    void Update(Comment comment);
    void Delete(Comment comment);
    Task<IReadOnlyCollection<Comment>> GetByIssueId(Guid issueId);
    Task<Comment?> GetById(Guid id);
}
