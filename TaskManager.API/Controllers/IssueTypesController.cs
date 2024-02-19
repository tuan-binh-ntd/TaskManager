namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class IssueTypesController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueTypeViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid projectId, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetIssueTypesByProjectIdQuery>
        .From(new GetIssueTypesByProjectIdQuery(projectId, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost]
    [ProducesResponseType(typeof(IssueTypeViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateIssueTypeDto createIssueTypeDto)
        => await Result.Success(new CreateIssueTypeCommand(createIssueTypeDto, projectId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{issueTypeId:guid}")]
    [ProducesResponseType(typeof(IssueTypeViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid issueTypeId, [FromBody] UpdateIssueTypeDto updateIssueTypeDto)
        => await Result.Success(new UpdateIssueTypeCommand(issueTypeId, updateIssueTypeDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{issueTypeId:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid issueTypeId, [FromQuery] Guid? newIssueTypeId)
        => await Result.Success(new DeleteIssueTypeCommand(issueTypeId, newIssueTypeId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
