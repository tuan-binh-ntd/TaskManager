﻿using CoreApiResponse;
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
        [ProducesResponseType(typeof(IReadOnlyCollection<AttachmentViewModel>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid issueId, List<IFormFile> files)
        {
            var res = await _attachmentService.CreateMultiple(issueId, files);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _attachmentService.Delete(id);
            return CustomResult(res, HttpStatusCode.OK);
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadBlobs(Guid issueId, List<IFormFile> files)
        //{
        //    var response = await _attachmentService.UploadFiles(issueId, files);
        //    return Ok(response);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllBlobs()
        //{
        //    var response = await _attachmentService.GetUploadedBlobs();
        //    return Ok(response);
        //}
    }
}
