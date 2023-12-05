using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
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
        private readonly IEmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            IEmailSender emailSender,
            UserManager<AppUser> userManager,
            IIssueRepository issueRepository,
            IMapper mapper
            )
        {
            _commentRepository = commentRepository;
            _emailSender = emailSender;
            _userManager = userManager;
            _issueRepository = issueRepository;
            _mapper = mapper;
        }

        public async Task<CommentViewModel> CreateComment(Guid issueId, CreateCommentDto createCommentDto)
        {
            var comment = _mapper.Map<Comment>(createCommentDto);
            comment.IssueId = issueId;
            _commentRepository.Add(comment);
            await _commentRepository.UnitOfWork.SaveChangesAsync();

            var issue = await _issueRepository.Get(issueId);

            var senderName = await _userManager.Users.Where(u => u.Id == createCommentDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
            var projectName = await _issueRepository.GetProjectNameOfIssue(issueId);

            var addNewCommentIssueEmailContentDto = new AddNewCommentIssueEmailContentDto()
            {
                ReporterName = senderName,
                IssueCreationTime = DateTime.Now,
                CommentContent = createCommentDto.Content,
            };

            string emailContent = EmailContentConstants.AddNewCommentIssueContent(addNewCommentIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto()
            {
                SenderName = senderName,
                ActionName = EmailConstants.MadeOneUpdate,
                ProjectName = projectName,
                IssueCode = issue.Code,
                IssueName = issue.Name,
                EmailContent = emailContent,
            };

            await _emailSender.SendEmailWhenCreatedIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: createCommentDto.CreatorUserId, buidEmailTemplateBaseDto);
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
