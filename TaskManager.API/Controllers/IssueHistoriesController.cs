using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/issues/{issueId}/[controller]")]
    [ApiController]
    public class IssueHistoriesController : BaseController
    {
        private readonly IIssueService _issueService;

        public IssueHistoriesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<IssueHistoryViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid issueId)
        {
            var res = await _issueService.GetHistoriesByIssueId(issueId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
