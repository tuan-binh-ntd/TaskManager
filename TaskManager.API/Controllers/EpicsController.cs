using CoreApiResponse;
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

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class EpicsController : BaseController
{
    private readonly IEpicService _epicService;
    private readonly PresenceTracker _presenceTracker;
    private readonly IHubContext<NotificationHub> _hubContext;

    public EpicsController(
        IEpicService epicService,
        PresenceTracker presenceTracker,
        IHubContext<NotificationHub> hubContext
        )
    {
        _epicService = epicService;
        _presenceTracker = presenceTracker;
        _hubContext = hubContext;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EpicViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateEpicDto createEpicDto)
    {
        var res = await _epicService.CreateEpic(createEpicDto);

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Epic, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EpicViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateEpicDto updateEpicDto)
    {
        var res = await _epicService.UpdateEpic(id, updateEpicDto);

        var connectionIds = _presenceTracker.GetConnectionsForUserIds(res.UserIds);
        await _hubContext.Clients.Clients(connectionIds).SendAsync("NewNotification", res.Notification);

        return CustomResult(res.Epic, HttpStatusCode.OK);
    }

    [HttpPut("{id}/issues:add")]
    [ProducesResponseType(typeof(EpicViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddIssue(Guid id, AddIssueToEpicDto addIssueToEpicDto)
    {
        var res = await _epicService.AddIssueToEpic(issueId: addIssueToEpicDto.IssueId, epicId: id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var res = await _epicService.DeleteEpic(id, userId);
        return CustomResult(res.IssueId, HttpStatusCode.OK);
    }
}
