namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BacklogsController(
    IMediator mediator)
    : ApiController(mediator)
{

    [HttpPost("{backlogId:guid}/issues/:name")]
    [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateIssueByName(Guid backlogId, CreateIssueByNameDto createIssueByNameDto)
        => await Result.Success(new CreateIssueCommand(null, backlogId, createIssueByNameDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPatch("{backlogId:guid}/issues/{id:guid}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
        => await Result.Success(new UpdateIssueCommand(id, updateIssueDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
