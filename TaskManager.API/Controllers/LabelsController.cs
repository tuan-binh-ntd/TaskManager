namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class LabelsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<LabelViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetLabelsByProjectIdQuery>
        .From(new GetLabelsByProjectIdQuery(projectId, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost]
    [ProducesResponseType(typeof(LabelViewModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(Guid projectId, CreateLabelDto createLabelDto)
        => await Result.Success(new CreateLabelCommand(projectId, createLabelDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(LabelViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateLabelDto updateLabelDto)
        => await Result.Success(new UpdateLabelCommand(id, updateLabelDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
        => await Result.Success(new DeleteLabelCommand(id))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
