namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class SprintsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateSprintDto updateSprintDto)
        => await Result.Success(new UpdateSprintCommand(id, updateSprintDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPost]
    [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid projectId)
        => await Result.Success(new CreateSprintCommand(projectId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
        => await Result.Success(new DeleteSprintCommand(id))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}:start")]
    [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Start(Guid id, UpdateSprintDto updateSprintDto)
        => await Result.Success(new StartSprintCommand(id, updateSprintDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}:complete")]
    [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Complete(Guid id, Guid projectId, CompleteSprintDto completeSprintDto)
        => await Result.Success(new CompleteSprintCommand(projectId, id, completeSprintDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
        => await Maybe<GetSprintByIdQuery>
        .From(new GetSprintByIdQuery(id))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost("get-sprints")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SprintViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId, [FromBody] GetSprintByFilterDto getSprintByFilterDto)
        => await Maybe<GetSprintsForBoardQuery>
        .From(new GetSprintsForBoardQuery(projectId, getSprintByFilterDto))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
