using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateIssueTypeDto createIssueTypeDto)
        {
            var res = await _issueTypeService.CreateIssueType(projectId, createIssueTypeDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{issueTypeId}")]
        public async Task<IActionResult> Update(Guid issueTypeId, [FromBody] UpdateIssueTypeDto updateIssueTypeDto)
        {
            var res = await _issueTypeService.UpdateIssueType(issueTypeId, updateIssueTypeDto);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
