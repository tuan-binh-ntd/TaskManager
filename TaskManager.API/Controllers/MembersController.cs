namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class MembersController : BaseController
{
    private readonly IProjectService _projectService;

    public MembersController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<MemberProjectViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(PaginationResult<MemberProjectViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetMembers(Guid projectId, [FromQuery] PaginationInput paginationInput)
    {
        var res = await _projectService.GetMembersOfProject(projectId, paginationInput);

        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MemberProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberProjectDto updateMemberProjectDto)
    {
        var res = await _projectService.UpdateMembder(id, updateMemberProjectDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid projectId, Guid id)
    {
        var res = await _projectService.DeleteMember(projectId, id);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
