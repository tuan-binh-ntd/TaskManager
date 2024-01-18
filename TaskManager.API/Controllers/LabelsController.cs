using TaskManager.Core.Helper;

namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class LabelsController : BaseController
{
    private readonly ILabelService _labelService;

    public LabelsController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<LabelViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(PaginationResult<LabelViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
    {
        var res = await _labelService.GetLabelsByProjectId(projectId, paginationInput);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LabelViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid projectId, CreateLabelDto createLabelDto)
    {
        var res = await _labelService.Create(projectId, createLabelDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(LabelViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateLabelDto updateLabelDto)
    {
        var res = await _labelService.Update(id, updateLabelDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var res = await _labelService.Delete(id);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
