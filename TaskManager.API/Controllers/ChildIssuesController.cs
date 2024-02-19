namespace TaskManager.API.Controllers;

[ApiController]
public class ChildIssuesController(IMediator mediator)
    : ApiController(mediator)
{
    //[HttpPost("api/issues/{projectId:guid}/[controller]")]
    //[ProducesResponseType(typeof(ChildIssueViewModel), (int)HttpStatusCode.Created)]
    //public async Task<IActionResult> Create(CreateChildIssueDto createChildIssueDto)
    //{
    //    var res = await _issueService.CreateChildIssue(createChildIssueDto);
    //    return CustomResult(res, HttpStatusCode.Created);
    //}

    //[HttpGet("api/epics/{epicId:guid}/[controller]")]
    //[ProducesResponseType(typeof(GetIssuesByEpicIdViewModel), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetIssuesForEpic(Guid epicId)
    //{
    //    var res = await _epicService.GetIssuesByEpicId(epicId);
    //    return CustomResult(res, HttpStatusCode.OK);
    //}
}
