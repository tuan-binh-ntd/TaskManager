using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.API.Extensions;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/issues/{issueId}/[controller]")]
[ApiController]
public class CommentsController : BaseController
{
    private readonly ICommentService _commentService;

    public CommentsController(
        ICommentService commentService
        )
    {
        _commentService = commentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CommentViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid issueId)
    {
        var res = await _commentService.GetCommentsByIssueId(issueId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CommentViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid issueId, CreateCommentDto createCommentDto)
    {
        var res = await _commentService.CreateComment(issueId, createCommentDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid issueId, Guid id, UpdateCommentDto updateCommentDto)
    {
        var res = await _commentService.UpdateComment(id, updateCommentDto, issueId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid issueId, Guid id)
    {
        var userId = User.GetUserId();
        var res = await _commentService.DeleteComment(issueId, id, userId);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
