namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class PrioritiesController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetPrioritiesByProjectIdQuery>
        .From(new GetPrioritiesByProjectIdQuery(projectId, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, BadRequest);

    [HttpPost]
    [ProducesResponseType(typeof(PriorityViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreatePriorityDto createPriorityDto)
        => await Result.Success(new CreatePriorityCommand(createPriorityDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PriorityViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdatePriorityDto updatePriorityDto)
        => await Result.Success(new UpdatePriorityCommand(id, updatePriorityDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid newId)
        => await Result.Success(new DeletePriorityCommand(id, newId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
