namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BacklogsController : BaseController
{
    private readonly IIssueService _issueService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly PresenceTracker _presenceTracker;

    public BacklogsController(
        IIssueService issueService,
        IHubContext<NotificationHub> hubContext,
        PresenceTracker presenceTracker
        )
    {
        _issueService = issueService;
        _hubContext = hubContext;
        _presenceTracker = presenceTracker;
    }

    [HttpGet("{backlogId}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByBacklogId(Guid backlogId)
    {
        var res = await _issueService.GetByBacklogId(backlogId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("{backlogId}/issues")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateIssue(Guid backlogId, CreateIssueDto createIssueDto)
    {
        var res = await _issueService.CreateIssue(createIssueDto, sprintId: null, backlogId: backlogId);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPost("{backlogId}/issues/:name")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateIssueByName(Guid backlogId, CreateIssueByNameDto createIssueByNameDto)
    {
        var res = await _issueService.CreateIssueByName(createIssueByNameDto, sprintId: null, backlogId: backlogId);

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Issue, HttpStatusCode.Created);
    }

    [HttpPatch("{backlogId}/issues/{id}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
    {
        var res = await _issueService.UpdateIssue(id, updateIssueDto);

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Issue, HttpStatusCode.OK);
    }
}
