using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            IMapper mapper
            )
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentViewModel> CreateComment(Guid issueId, CreateCommentDto createCommentDto)
        {
            var comment = _mapper.Map<Comment>(createCommentDto);
            comment.IssueId = issueId;
            _commentRepository.Add(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return comment.Adapt<CommentViewModel>();
        }

        public async Task<Guid> DeleteComment(Guid id)
        {
            _commentRepository.Delete(id);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueId(Guid issueId)
        {
            var comments = await _commentRepository.GetByIssueId(issueId);
            return comments.Adapt<IReadOnlyCollection<CommentViewModel>>();
        }

        public async Task<CommentViewModel> UpdateComment(Guid id, UpdateCommentDto updateCommentDto)
        {
            var comment = await _commentRepository.GetById(id) ?? throw new CommentNullException();
            comment.Content = updateCommentDto.Content;
            _commentRepository.Update(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();
            return comment.Adapt<CommentViewModel>();
        }
    }
}
