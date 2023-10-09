using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/projectId/[controller]")]
    [ApiController]
    public class IssueTypesController : BaseController
    {
        private readonly IIssueTypeService _issueTypeService;

        public IssueTypesController(IIssueTypeService issueTypeService)
        {
            _issueTypeService = issueTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Gets(Guid projectId)
        {
            var res = await _issueTypeService.GetIssueTypesByProjectId(projectId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
