using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

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
        public async Task<IActionResult> Create(CreateIssueDto createIssueDto)
        {
            var res = await _issueService.CreateIssue(createIssueDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateIssueDto updateIssueDto)
        {
            var res = await _issueService.UpdateIssue(id, updateIssueDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<IActionResult> GetBySprintId(Guid sprintId)
        {
            var res = await _issueService.GetBySprintId(sprintId);
            return CustomResult(res, HttpStatusCode.OK);
        }

        
    }
}
