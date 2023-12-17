using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using TaskManager.API.Hubs;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[ApiController]
public class IssuesController : BaseController
{
    private readonly IIssueService _issueService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public IssuesController(
        IIssueService issueService,
        IHubContext<NotificationHub> hubContext
        )
    {
        _issueService = issueService;
        _hubContext = hubContext;
    }

    [HttpPost("api/sprints/{sprintId}/[controller]")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid sprintId, CreateIssueDto createIssueDto)
    {
        var res = await _issueService.CreateIssue(createIssueDto, sprintId: sprintId, null);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("api/sprints/{sprintId}/[controller]/{id}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateIssueDto updateIssueDto)
    {
        var res = await _issueService.UpdateIssue(id, updateIssueDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/sprints/{sprintId}/[controller]")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBySprintId(Guid sprintId)
    {
        var res = await _issueService.GetBySprintId(sprintId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("api/sprints/{sprintId}/[controller]/:name")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateIssueByName(Guid sprintId, CreateIssueByNameDto createIssueByNameDto)
    {
        var res = await _issueService.CreateIssueByName(createIssueByNameDto, sprintId: sprintId, null);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPatch("api/sprints/{sprintId}/[controller]/{id}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
    {
        var res = await _issueService.UpdateIssue(id, updateIssueDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("api/sprints/{sprintId}/[controller]/{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var res = await _issueService.DeleteIssue(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{id}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var res = await _issueService.GetById(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/projects/{projectId}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueForProjectViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var res = await _issueService.GetIssuesForProject(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("api/[controller]/{id}/labels:delete")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteLabel(Guid id, [FromQuery] Guid labelId)
    {
        var res = await _issueService.DeleteLabelToIssue(id, labelId);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
