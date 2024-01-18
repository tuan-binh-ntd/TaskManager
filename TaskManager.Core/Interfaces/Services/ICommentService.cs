namespace TaskManager.Core.Interfaces.Services;

public interface ICommentService
{
    Task<CommentViewModel> CreateComment(Guid issueId, CreateCommentDto createCommentDto);
    Task<CommentViewModel> UpdateComment(Guid id, UpdateCommentDto updateCommentDto, Guid issueId);
    Task<Guid> DeleteComment(Guid issueId, Guid id, Guid userId);
    Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueId(Guid issueId);
}
