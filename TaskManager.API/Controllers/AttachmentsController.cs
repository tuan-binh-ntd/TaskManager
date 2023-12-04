using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.API.Extensions;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/issues/{issueId}/[controller]")]
    [ApiController]
    public class AttachmentsController : BaseController
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IReadOnlyCollection<AttachmentViewModel>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid issueId, List<IFormFile> files)
        {
            var userId = User.GetUserId();
            var res = await _attachmentService.CreateMultiple(issueId, files, userId);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id, Guid issueId)
        {
            var userId = User.GetUserId();
            var res = await _attachmentService.Delete(id, userId, issueId);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<AttachmentViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Gets(Guid issueId)
        {
            var res = await _attachmentService.GetByIssueId(issueId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
