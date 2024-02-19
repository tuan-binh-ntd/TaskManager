namespace TaskManager.API.Controllers;

[Route("api/issues/{issueId:guid}/[controller]")]
public class AttachmentsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpPost]
    [ProducesResponseType(typeof(IReadOnlyCollection<AttachmentViewModel>), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid issueId, List<IFormFile> files)
    {
        var userId = User.GetUserId();
        return await Result.Success(new CreateAttachmentCommand(issueId, files, userId))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id, Guid issueId)
    {
        var userId = User.GetUserId();
        return await Result.Success(new DeleteAttachmentCommand(id, userId, issueId))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<AttachmentViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid issueId)
        => await Maybe<GetAttachmentsByIssueIdQuery>
        .From(new GetAttachmentsByIssueIdQuery(issueId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
