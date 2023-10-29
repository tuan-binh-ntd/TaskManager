using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/issues/{issueId}/[controller]")]
    [ApiController]
    public class CommentsController : BaseController
    {
        private readonly IIssueService _issueService;

        public CommentsController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<CommentViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid issueId)
        {
            var res = await _issueService.GetCommentsByIssueId(issueId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
