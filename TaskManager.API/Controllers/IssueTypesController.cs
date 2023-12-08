using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class IssueTypesController : BaseController
    {
        private readonly IIssueTypeService _issueTypeService;

        public IssueTypesController(IIssueTypeService issueTypeService)
        {
            _issueTypeService = issueTypeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<IssueTypeViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Gets(Guid projectId, [FromQuery] PaginationInput paginationInput)
        {
            var res = await _issueTypeService.GetIssueTypesByProjectId(projectId, paginationInput);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IssueTypeViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateIssueTypeDto createIssueTypeDto)
        {
            var res = await _issueTypeService.CreateIssueType(projectId, createIssueTypeDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{issueTypeId}")]
        [ProducesResponseType(typeof(IssueTypeViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid issueTypeId, [FromBody] UpdateIssueTypeDto updateIssueTypeDto)
        {
            var res = await _issueTypeService.UpdateIssueType(issueTypeId, updateIssueTypeDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("issueTypeId")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid issueTypeId, [FromQuery] Guid newIssueTypeId)
        {
            var res = await _issueTypeService.Delete(issueTypeId, newIssueTypeId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
