using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        [ProducesResponseType(typeof(AttachmentViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid issueId, IFormFile file)
        {
            var res = await _attachmentService.Create(issueId, file);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _attachmentService.Delete(id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
