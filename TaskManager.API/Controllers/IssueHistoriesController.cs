namespace TaskManager.API.Controllers;

[Route("api/issues/{issueId}/[controller]")]
[ApiController]
public class IssueHistoriesController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueHistoryViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid issueId)
        => await Maybe<GetIssueHistoriesByIssueIdQuery>
        .From(new GetIssueHistoriesByIssueIdQuery(issueId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
