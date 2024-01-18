namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class WorkflowsController : BaseController
{
    private readonly IWorkflowService _workflowService;

    public WorkflowsController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(WorkflowViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var res = await _workflowService.GetWorkflowViewModelByProjectId(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Update(Guid projectId, UpdateWorkflowDto updateWorkflowDto)
    {
        await _workflowService.UpdateWorkflow(projectId, updateWorkflowDto);
        return CustomResult(HttpStatusCode.OK);
    }
}
