﻿using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/users/{id}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(
            IProjectService projectService
            )
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectByFilter(Guid id, [FromQuery] GetProjectByFilterDto filter)
        {
            var result = await _projectService.GetProjectsByFilter(id, filter);
            return CustomResult(result, HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Guid id, CreateProjectDto createProjectDto)
        {
            var res = await _projectService.Create(id, createProjectDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, UpdateProjectDto updateProjectDto)
        {
            var res = await _projectService.Update(projectId, updateProjectDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var result = await _projectService.Delete(projectId);
            return CustomResult(result, HttpStatusCode.OK);
        }
    }
}
