﻿using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[ApiController]
public class ProjectsController : BaseController
{
    private readonly IProjectService _projectService;

    public ProjectsController(
        IProjectService projectService
        )
    {
        _projectService = projectService;
    }

    [HttpGet("api/users/{id}/[controller]"), AllowAnonymous]
    public async Task<IActionResult> GetProjectByFilter(Guid id, [FromQuery] GetProjectByFilterDto filter, [FromQuery] PaginationInput paginationInput)
    {
        var result = await _projectService.GetProjectsByFilter(id, filter, paginationInput);
        return CustomResult(result, HttpStatusCode.OK);
    }

    [HttpPost("api/users/{id}/[controller]"), AllowAnonymous]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateProject(Guid id, CreateProjectDto createProjectDto)
    {
        var res = await _projectService.Create(id, createProjectDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("api/users/{id}/[controller]/{projectId}")]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProject(Guid id, Guid projectId, UpdateProjectDto updateProjectDto)
    {
        var res = await _projectService.Update(userId: id, projectId, updateProjectDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("api/users/{id}/[controller]/{projectId}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var result = await _projectService.Delete(projectId);
        return CustomResult(result, HttpStatusCode.OK);
    }

    [HttpGet("api/users/{id}/[controller]/{code}"), AllowAnonymous]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string code, Guid id)
    {
        var result = await _projectService.Get(code, id);
        if (result is null)
        {
            return CustomResult(result, HttpStatusCode.NoContent);
        }
        return CustomResult(result, HttpStatusCode.OK);
    }

    [HttpPost("api/users/{id}/[controller]/members:add")]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddMember([FromBody] AddMemberToProjectDto addMemberToProjectDto)
    {
        var res = await _projectService.AddMember(addMemberToProjectDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPatch("api/users/{id}/[controller]/{projectId}")]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, Guid projectId, UpdateProjectDto updateProjectDto)
    {
        var res = await _projectService.Update(userId: id, projectId, updateProjectDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/sprint-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SprintFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSprintFilter(Guid projectId)
    {
        var res = await _projectService.GetSprintFiltersViewModel(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/epic-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<EpicFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetEpicFilter(Guid projectId)
    {
        var res = await _projectService.GetEpicFiltersViewModel(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/type-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TypeFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypeFilter(Guid projectId)
    {
        var res = await _projectService.GetTypeFiltersViewModel(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/label-filter")]
    [ProducesResponseType(typeof(IReadOnlyCollection<LabelFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLabelFilter(Guid projectId)
    {
        var res = await _projectService.GetLabelFiltersViewModel(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("api/[controller]/{projectId}/backlog")]
    [ProducesResponseType(typeof(GetIssueForProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid projectId, GetIssueForProjectFilterInputModel getIssueForProjectFilterInputModel)
    {
        var res = await _projectService.GetIssueForProjectViewModelAsync(projectId, getIssueForProjectFilterInputModel);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/version-filter-backlog")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SprintFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetVersionFilterForBacklog(Guid projectId)
    {
        var res = await _projectService.GetVerionFiltersViewModelForBacklog(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/epic-filter-backlog")]
    [ProducesResponseType(typeof(IReadOnlyCollection<EpicFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetEpicFilterForBacklog(Guid projectId)
    {
        var res = await _projectService.GetEpicFiltersViewModelForBacklog(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/type-filter-backlog")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TypeFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypeFilterForBacklog(Guid projectId)
    {
        var res = await _projectService.GetTypeFiltersViewModelForBacklog(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("api/[controller]/{projectId}/label-filter-backlog")]
    [ProducesResponseType(typeof(IReadOnlyCollection<LabelFilterViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLabelFilterForBacklog(Guid projectId)
    {
        var res = await _projectService.GetLabelFiltersViewModelForBacklog(projectId);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
