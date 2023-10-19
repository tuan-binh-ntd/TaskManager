using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/issues/[controller]")]
    [ApiController]
    public class ChildIssuesController : BaseController
    {
        private readonly IssueService _issueService;

        public ChildIssuesController(IssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ChildIssueViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(CreateChildIssueDto createChildIssueDto)
        {
            var res = await _issueService.CreateChildIssue(createChildIssueDto);
            return CustomResult(res, HttpStatusCode.Created);
        }
    }
}
