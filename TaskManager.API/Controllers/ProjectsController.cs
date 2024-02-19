namespace TaskManager.API.Controllers;

[ApiController]
public class ProjectsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet("api/users/{id:guid}/[controller]"), AllowAnonymous]
    public async Task<IActionResult> GetProjectByFilter(Guid id, [FromQuery] GetProjectByFilterDto filter, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetProjectsByFilterQuery>
        .From(new GetProjectsByFilterQuery(id, filter, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost("api/users/{id:guid}/[controller]"), AllowAnonymous]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateProject(Guid id, CreateProjectDto createProjectDto)
        => await Result.Success(new CreateProjectCommand(id, createProjectDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("api/users/{id:guid}/[controller]/{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProject(Guid id, Guid projectId, UpdateProjectDto updateProjectDto)
        => await Result.Success(new UpdateProjectCommand(id, projectId, updateProjectDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("api/users/{id:guid}/[controller]/{projectId:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProject(Guid projectId)
        => await Result.Success(new DeleteProjectCommand(projectId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpGet("api/users/{id:guid}/[controller]/{code}"), AllowAnonymous]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string code, Guid id)
        => await Maybe<GetProjectByCodeQuery>
        .From(new GetProjectByCodeQuery(id, code))
        .Binding(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPatch("api/users/{id:guid}/[controller]/{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, Guid projectId, UpdateProjectDto updateProjectDto)
    => await Result.Success(new UpdateProjectCommand(id, projectId, updateProjectDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpGet("api/[controller]/{projectId:guid}/sprint-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SprintFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSprintFilter(Guid projectId)
        => await Maybe<GetSprintFiltersViewModelsQuery>
        .From(new GetSprintFiltersViewModelsQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpGet("api/[controller]/{projectId:guid}/epic-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<EpicFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetEpicFilter(Guid projectId)
        => await Maybe<GetEpicFiltersViewModelsQuery>
        .From(new GetEpicFiltersViewModelsQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpGet("api/[controller]/{projectId:guid}/type-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TypeFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypeFilter(Guid projectId)
    => await Maybe<GetTypeFiltersViewModelsQuery>
        .From(new GetTypeFiltersViewModelsQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpGet("api/[controller]/{projectId:guid}/label-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<LabelFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLabelFilter(Guid projectId)
    => await Maybe<GetLabelFiltersViewModelsQuery>
        .From(new GetLabelFiltersViewModelsQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
