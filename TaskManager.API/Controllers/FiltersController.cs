namespace TaskManager.API.Controllers;

[ApiController]
public class FiltersController(IMediator mediator)
    : ApiController(mediator)
{
    [Authorize]
    [HttpGet("api/[controller]/{id:guid}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var userId = User.GetUserId();
        var query = new GetIssuesByFilterConfigurationQuery(id, userId);
        var response = await Mediator.Send(query);
        return CustomResult(response, HttpStatusCode.OK);
    }

    [HttpPost("api/[controller]")]
    [ProducesResponseType(typeof(FilterViewModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateFilterDto createFilterDto)
        => await Result.Success(new CreateFilterCommand(createFilterDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("api/[controller]/{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
        => await Result.Success(new DeleteFilterCommand(id))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPost("api/[controller]/get-issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromBody] GetIssueByConfigurationDto getIssueByConfigurationDto)
        => await Maybe<GetIssuesByConfigurationQuery>
        .From(new GetIssuesByConfigurationQuery(getIssueByConfigurationDto))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpGet("api/users/{userId:guid}/[controller]")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FilterViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Gets(Guid userId)
        => await Maybe<GetFilterViewModelsByUserIdQuery>
        .From(new GetFilterViewModelsByUserIdQuery(userId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
