using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BacklogsController : BaseController
{
    private readonly IIssueService _issueService;

    public BacklogsController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    [HttpGet("{backlogId}/issues")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IssueViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByBacklogId(Guid backlogId)
    {
        var res = await _issueService.GetByBacklogId(backlogId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("{backlogId}/issues")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateIssue(Guid backlogId, CreateIssueDto createIssueDto)
    {
        var res = await _issueService.CreateIssue(createIssueDto, sprintId: null, backlogId: backlogId);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPost("{backlogId}/issues/:name")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateIssueByName(Guid backlogId, CreateIssueByNameDto createIssueByNameDto)
    {
        var res = await _issueService.CreateIssueByName(createIssueByNameDto, sprintId: null, backlogId: backlogId);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPatch("{backlogId}/issues/{id}")]
    [ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Patch(Guid id, UpdateIssueDto updateIssueDto)
    {
        var res = await _issueService.UpdateIssue(id, updateIssueDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    //[HttpGet("{backlogId}/issues/{id}")]
    //[ProducesResponseType(typeof(IssueViewModel), (int)HttpStatusCode.OK)]
    //public async Task<IActionResult> GetById(Guid id)
    //{
    //    var res = await _issueService.GetById(id);
    //    return CustomResult(res, HttpStatusCode.OK);
    //}
}
