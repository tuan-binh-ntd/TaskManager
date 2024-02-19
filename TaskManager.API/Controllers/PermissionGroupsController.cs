namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class PermissionGroupsController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<PermissionGroupViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetPermissionGroupsByProjectIdQuery>
        .From(new GetPermissionGroupsByProjectIdQuery(projectId, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost]
    [ProducesResponseType(typeof(PermissionGroupViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] CreatePermissionGroupDto createPermissionGroupDto)
        => await Result.Success(new CreatePermissionGroupCommand(createPermissionGroupDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PermissionGroupViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionGroupDto updatePermissionGroupDto)
        => await Result.Success(new UpdatePermissionGroupCommand(id, updatePermissionGroupDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid? newPermissionGroupId)
        => await Result.Success(new DeletePermissionGroupCommand(id, newPermissionGroupId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
