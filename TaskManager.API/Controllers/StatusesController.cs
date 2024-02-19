using System.ComponentModel.DataAnnotations;
using TaskManager.Application.Statuses.Commands.Create;
using TaskManager.Application.Statuses.Commands.Delete;
using TaskManager.Application.Statuses.Commands.Update;
using TaskManager.Application.Statuses.Queries.GetStatusesByProjectId;

namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class StatusesController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpPost]
    [ProducesResponseType(typeof(StatusViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateStatusDto createStatusDto)
        => await Result.Success(new CreateStatusCommand(createStatusDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(StatusViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateStatusDto updateStatusDto)
    => await Result.Success(new UpdateStatusCommand(id, updateStatusDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery, Required] Guid newId)
    => await Result.Success(new DeleteStatusCommand(id, newId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<StatusViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetStatusesByProjectIdQuery>
        .From(new GetStatusesByProjectIdQuery(projectId, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, BadRequest);
}
