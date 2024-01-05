using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.API.Extensions;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[ApiController]
public class FiltersController : BaseController
{
    private readonly IFilterService _filterService;

    public FiltersController(IFilterService filterService)
    {
        _filterService = filterService;
    }

    [Authorize]
    [HttpGet("api/[controller]/{id}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var userId = User.GetUserId();
        var res = await _filterService.GetIssueByFilterConfiguration(id, userId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("api/[controller]")]
    [ProducesResponseType(typeof(FilterViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateFilterDto createFilterDto)
    {
        var res = await _filterService.CreateFilter(createFilterDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpDelete("api/[controller]/{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var res = await _filterService.DeleteFilter(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("api/[controller]/get-issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get([FromBody] GetIssueByConfigurationDto getIssueByConfigurationDto)
    {
        var res = await _filterService.GetIssuesByConfiguration(getIssueByConfigurationDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/users/{userId}/[controller]")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid userId)
    {
        var res = await _filterService.GetFilterViewModelsByUserId(userId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPut("api/[controller]/{id}")]
    [ProducesResponseType(typeof(FilterViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateFilterDto updateFilterDto)
    {
        var res = await _filterService.UpdateFilter(id, updateFilterDto);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
