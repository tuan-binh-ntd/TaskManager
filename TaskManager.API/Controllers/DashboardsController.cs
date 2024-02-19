namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class DashboardsController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpGet("circle-chart")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
        => await Maybe<GetIssueNumOfAssigneeDashboardQuery>
        .From(new GetIssueNumOfAssigneeDashboardQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpGet("column-chart")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssuesInProjectDashboardViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid projectId)
        => await Maybe<GetIssuesInProjectDashboardQuery>
        .From(new GetIssuesInProjectDashboardQuery(projectId))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost("table-chart")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetIssueViewModels(Guid projectId, [FromBody] GetIssuesForAssigneeOrReporterDto getIssuesForAssigneeOrReporterDto)
        => await Maybe<GetIssueViewModelDashboardQuery>
        .From(new GetIssueViewModelDashboardQuery(projectId, getIssuesForAssigneeOrReporterDto))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
