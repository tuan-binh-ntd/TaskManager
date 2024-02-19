namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class VersionsController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<VersionViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
        => await Maybe<GetVersionsByProjectIdQuery>
        .From(new GetVersionsByProjectIdQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost]
    [ProducesResponseType(typeof(VersionViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateVersionDto createVersionDto)
    => await Result.Success(new CreateVersionCommand(createVersionDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VersionViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateVersionDto updateVersionDto)
        => await Result.Success(new UpdateVersionCommand(id, updateVersionDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
        => await Result.Success(new DeleteVersionCommand(id))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
