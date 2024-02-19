namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IssueEventsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueEventViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Gets()
        => await Maybe<GetAllIssueEventsQuery>
        .From(new GetAllIssueEventsQuery())
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
