using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class StatusesController : BaseController
{
    private readonly IStatusService _statusService;

    public StatusesController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(StatusViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateStatusDto createStatusDto)
    {
        var res = await _statusService.Create(createStatusDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(StatusViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateStatusDto updateStatusDto)
    {
        var res = await _statusService.Update(id, updateStatusDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Delete(Guid id, [FromQuery, Required] Guid newId)
    {
        var res = await _statusService.Delete(id, newId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<StatusViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
    {
        var res = await _statusService.Gets(projectId, paginationInput);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("versions")]
    [ProducesResponseType(typeof(IReadOnlyCollection<StatusViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid projectId)
    {
        var res = await _statusService.GetStatusViewModelsForViewAsync(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
