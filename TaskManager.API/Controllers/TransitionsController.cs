namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class TransitionsController : BaseController
{
    private readonly ITransitionService _transitionService;

    public TransitionsController(ITransitionService transitionService)
    {
        _transitionService = transitionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TransitionViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var res = await _transitionService.GetTransitionViewModelByProjectId(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransitionViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid projectId, CreateTransitionDto createTransitionDto)
    {
        var res = await _transitionService.CreateTransition(projectId, createTransitionDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TransitionViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateTransitionDto updateTransitionDto)
    {
        var res = await _transitionService.UpdateTransition(id, updateTransitionDto);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
