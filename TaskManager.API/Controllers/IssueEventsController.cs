using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IssueEventsController : BaseController
{
    private readonly INotificationEventService _notificationEventService;

    public IssueEventsController(INotificationEventService notificationEventService)
    {
        _notificationEventService = notificationEventService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueEventViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets()
    {
        var res = await _notificationEventService.GetIssueEventViewModels();
        return CustomResult(res, HttpStatusCode.OK);
    }
}
