namespace TaskManager.API.Controllers;

[ApiController]
public class IssuesController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpPut("api/sprints/{sprintId:guid}/[controller]/{id:guid}")]
    [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid sprintId, Guid id, UpdateIssueDto updateIssueDto)
        => await Result.Success(new UpdateIssueCommand(id, updateIssueDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpGet("api/sprints/{sprintId:guid}/[controller]")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySprintId(Guid sprintId)
        => await Maybe<GetIssuesBySprintIdQuery>
        .From(new GetIssuesBySprintIdQuery(sprintId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost("api/sprints/{sprintId:guid}/[controller]/:name")]
    [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateIssueByName(Guid sprintId, CreateIssueByNameDto createIssueByNameDto)
        => await Result.Success(new CreateIssueCommand(sprintId, null, createIssueByNameDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPatch("api/sprints/{sprintId:guid}/[controller]/{id:guid}")]
    [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
        => await Result.Success(new UpdateIssueCommand(id, updateIssueDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [Authorize]
    [HttpDelete("api/sprints/{sprintId:guid}/[controller]/{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var command = new DeleteIssueCommand(id, userId);
        var response = await Mediator.Send(command);
        return CustomResult(response);
    }

    [HttpGet("api/[controller]/{id:guid}")]
    [ProducesResponseType(typeof(IssueViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
        => await Maybe<GetByIdQuery>.From(new GetByIdQuery(id))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpGet("api/projects/{projectId:guid}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueForProjectViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid projectId)
        => await Maybe<GetIssuesForProjectQuery>
        .From(new GetIssuesForProjectQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
