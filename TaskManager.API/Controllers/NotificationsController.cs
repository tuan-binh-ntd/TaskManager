using TaskManager.Application.NotificationIssueEvents.Commands.Create;
using TaskManager.Application.NotificationIssueEvents.Commands.Delete;
using TaskManager.Application.NotificationIssueEvents.Commands.Update;

namespace TaskManager.API.Controllers;

[ApiController]
public class NotificationsController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost("api/[controller]/{id:guid}/notification-events")]
    [ProducesResponseType(typeof(NotificationEventViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(Guid id, CreateNotificationEventDto createNotificationEventDto)
        => await Result.Success(new CreateNotificationIssueEventCommand(id, createNotificationEventDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("api/[controller]/{id:guid}/notification-events/{notificationEventId}")]
    [ProducesResponseType(typeof(NotificationEventViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid notificationEventId, UpdateNotificationEventDto updateNotificationEventDto)
        => await Result.Success(new UpdateNotificationIssueEventCommand(notificationEventId, updateNotificationEventDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("api/[controller]/{id:guid}/notification-events/{notificationEventId}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid notificationEventId)
        => await Result.Success(new DeleteNotificationIssueEventCommand(notificationEventId))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    //[HttpGet("api/projects/{projectId}/[controller]")]
    //[ProducesResponseType(typeof(NotificationViewModel), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetNotification(Guid projectId)
    //{
    //    var res = await _notificationEventService.GetNotificationViewModelByProjectId(projectId);
    //    return CustomResult(res, HttpStatusCode.OK);
    //}
}
