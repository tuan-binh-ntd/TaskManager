using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
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
    }
}
