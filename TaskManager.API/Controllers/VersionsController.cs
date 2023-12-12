using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId}/[controller]")]
[ApiController]
public class VersionsController : BaseController
{
    private readonly IVersionService _versionService;

    public VersionsController(IVersionService versionService)
    {
        _versionService = versionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<VersionViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId)
    {
        var res = await _versionService.GetByProjectId(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(VersionViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateVersionDto createVersionDto)
    {
        var res = await _versionService.Create(createVersionDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VersionViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateVersionDto updateVersionDto)
    {
        var res = await _versionService.Update(id, updateVersionDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var res = await _versionService.Delete(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPut("{id}/issues:add")]
    [ProducesResponseType(typeof(VersionViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddIssues(AddIssuesToVersionDto addIssuesToVersionDto)
    {
        var res = await _versionService.AddIssues(addIssuesToVersionDto);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
