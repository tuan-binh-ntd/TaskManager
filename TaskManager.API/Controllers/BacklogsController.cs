using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BacklogsController : BaseController
    {
        private readonly IIssueService _issueService;

        public BacklogsController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("{id}/issues")]
        public async Task<IActionResult> GetByBacklogId(Guid backlogId)
        {
            var res = await _issueService.GetByBacklogId(backlogId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
