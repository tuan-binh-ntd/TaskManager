using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IReadOnlyCollection<CommentViewModel>> Gets();
        CommentViewModel Add(Comment comment);
        void Update(Comment comment);
        void Delete(Guid id);
        Task<IReadOnlyCollection<Comment>> GetByIssueId(Guid issueId);
    }
}
