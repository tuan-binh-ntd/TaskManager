using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/sprints/{sprintId}/[controller]")]
    [ApiController]
    public class IssuesController : BaseController
    {
        private readonly IIssueService _issueService;

        public IssuesController(
            IIssueService issueService
            )
        {
            _issueService = issueService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid sprintId, CreateIssueDto createIssueDto)
        {
            var res = await _issueService.CreateIssue(createIssueDto, sprintId: sprintId, null);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, UpdateIssueDto updateIssueDto)
        {
            var res = await _issueService.UpdateIssue(id, updateIssueDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBySprintId(Guid sprintId)
        {
            var res = await _issueService.GetBySprintId(sprintId);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost(":name")]
        [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateIssueByName(Guid sprintId, CreateIssueByNameDto createIssueByNameDto)
        {
            var res = await _issueService.CreateIssueByName(createIssueByNameDto, sprintId: sprintId, null);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
        {
            var res = await _issueService.UpdateIssue(id, updateIssueDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _issueService.DeleteIssue(id);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _issueService.GetById(id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
