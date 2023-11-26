﻿using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentViewModel> CreateComment(Guid issueId, CreateCommentDto createCommentDto);
        Task<CommentViewModel> UpdateComment(Guid id, UpdateCommentDto updateCommentDto);
        Task<Guid> DeleteComment(Guid id);
        Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueId(Guid issueId);
    }
}