﻿using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using TaskManager.API.Extensions;
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
    private readonly PresenceTracker _presenceTracker;

    public IssuesController(
        IIssueService issueService,
        IHubContext<NotificationHub> hubContext,
        PresenceTracker presenceTracker
        )
    {
        _issueService = issueService;
        _hubContext = hubContext;
        _presenceTracker = presenceTracker;
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

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Issue, HttpStatusCode.OK);
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

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Issue, HttpStatusCode.Created);
    }

    [HttpPatch("api/sprints/{sprintId}/[controller]/{id}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
    {
        var res = await _issueService.UpdateIssue(id, updateIssueDto);

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Issue, HttpStatusCode.OK);
    }

    [Authorize]
    [HttpDelete("api/sprints/{sprintId}/[controller]/{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var res = await _issueService.DeleteIssue(id, userId);
        return CustomResult(res.IssueId, HttpStatusCode.OK);
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
