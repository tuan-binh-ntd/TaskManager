using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [ApiController]
    public class ChildIssuesController : BaseController
    {
        private readonly IIssueService _issueService;
        private readonly IEpicService _epicService;

        public ChildIssuesController(
            IIssueService issueService,
            IEpicService epicService
            )
        {
            _issueService = issueService;
            _epicService = epicService;
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
        public async Task<IActionResult> Get(Guid epicId)
        {
            var res = await _epicService.GetIssuesByEpicId(epicId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
