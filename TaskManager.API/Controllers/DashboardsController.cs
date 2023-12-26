using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class DashboardsController : BaseController
{
    private readonly IDashboardService _dashboardService;

    public DashboardsController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var res = await _dashboardService.GetIssueOfAssigneeDashboardViewModelAsync(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("issues-in-project")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssuesInProjectDashboardViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid projectId)
    {
        var res = await _dashboardService.GetIssuesInProjectDashboardViewModelAsync(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetIssueViewModels(Guid projectId, [FromBody] GetIssuesForAssigneeOrReporterDto getIssuesForAssigneeOrReporterDto)
    {
        var res = await _dashboardService.GetIssueViewModelsDashboardViewModelAsync(projectId, getIssuesForAssigneeOrReporterDto);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
