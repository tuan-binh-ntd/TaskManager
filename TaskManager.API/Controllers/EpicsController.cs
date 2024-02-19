namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class EpicsController(
    IMediator mediator
        ) : ApiController(mediator)
{

    [HttpPost]
    [ProducesResponseType(typeof(EpicViewModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateEpicDto createEpicDto)
        => await Result.Success(new CreateEpicCommand(createEpicDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EpicViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, UpdateEpicDto updateEpicDto)
        => await Result.Success(new UpdateEpicCommand(id, updateEpicDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var command = new DeleteEpicCommand(id, userId);
        var response = await Mediator.Send(command);
        return CustomResult(response, HttpStatusCode.OK);
    }
}
