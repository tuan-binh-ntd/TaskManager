namespace TaskManager.API.Controllers;

[ApiController]
public class ChildIssuesController : BaseController
{
    private readonly IIssueService _issueService;
    private readonly IEpicService _epicService;
    private readonly IVersionService _versionService;

    public ChildIssuesController(
        IIssueService issueService,
        IEpicService epicService,
        IVersionService versionService
        )
    {
        _issueService = issueService;
        _epicService = epicService;
        _versionService = versionService;
    }

    [HttpPost("api/issues/{projectId}/[controller]")]
    [ProducesResponseType(typeof(ChildIssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateChildIssueDto createChildIssueDto)
    {
        var res = await _issueService.CreateChildIssue(createChildIssueDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpGet("api/epics/{epicId}/[controller]")]
    [ProducesResponseType(typeof(GetIssuesByEpicIdViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetIssuesForEpic(Guid epicId)
    {
        var res = await _epicService.GetIssuesByEpicId(epicId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/versions/{versionId}/[controller]")]
    [ProducesResponseType(typeof(GetIssuesByVersionIdViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetIssuesForVersion(Guid versionId)
    {
        var res = await _versionService.GetIssuesByVersionId(versionId);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
