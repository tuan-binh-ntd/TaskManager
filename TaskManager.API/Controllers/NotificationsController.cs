using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : BaseController
{
    private readonly INotificationEventService _notificationEventService;

    public NotificationsController(INotificationEventService notificationEventService)
    {
        _notificationEventService = notificationEventService;
    }

    [HttpGet("{id}/notificationevents")]
    [ProducesResponseType(typeof(IReadOnlyCollection<NotificationEventViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await _notificationEventService.GetNotificationEventsByNotificationId(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("{id}/notificationevents")]
    [ProducesResponseType(typeof(NotificationEventViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid id, CreateNotificationEventDto createNotificationEventDto)
    {
        var res = await _notificationEventService.Create(id, createNotificationEventDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}/notificationevents/{notificationEventId}")]
    [ProducesResponseType(typeof(NotificationEventViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid notificationEventId, UpdateNotificationEventDto updateNotificationEventDto)
    {
        var res = await _notificationEventService.Update(notificationEventId, updateNotificationEventDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("{id}/notificationevents/{notificationEventId}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task <IActionResult> Delete(Guid notificationEventId)
    {
        var res = await _notificationEventService.Delete(notificationEventId);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
