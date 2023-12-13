using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FiltersController : BaseController
{
    private readonly IFilterService _filterService;

    public FiltersController(IFilterService filterService)
    {
        _filterService = filterService;
    }

    [HttpGet("{id}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await _filterService.GetIssueByFilterConfiguration(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FilterViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateFilterDto createFilterDto)
    {
        var res = await _filterService.CreateFilter(createFilterDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var res = await _filterService.DeleteFilter(id);
        return CustomResult(res, HttpStatusCode.Created);
    }
}
