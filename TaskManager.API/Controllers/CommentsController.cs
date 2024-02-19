namespace TaskManager.API.Controllers;

[Route("api/issues/{issueId:guid}/[controller]")]
[ApiController]
public class CommentsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CommentViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid issueId)
        => await Maybe<GetCommentsByIssueIdQuery>
        .From(new GetCommentsByIssueIdQuery(issueId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost]
    [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Guid issueId, CreateCommentDto createCommentDto)
        => await Result.Success(new CreateCommentCommand(issueId, createCommentDto.Content, createCommentDto.CreatorUserId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CommentViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateCommentDto updateCommentDto)
        => await Result.Success(new UpdateCommentCommand(id, updateCommentDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid issueId, Guid id)
        => await Result.Success(new DeleteCommentCommand(id, issueId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
