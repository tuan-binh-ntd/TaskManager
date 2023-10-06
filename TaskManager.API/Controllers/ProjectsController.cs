﻿using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
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

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetProjectByFilter(Guid id, [FromQuery] GetProjectByFilterDto filter, [FromQuery] PaginationInput paginationInput)
        {
            var result = await _projectService.GetProjectsByFilter(id, filter, paginationInput);
            return CustomResult(result, HttpStatusCode.OK);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> CreateProject(Guid id, CreateProjectDto createProjectDto)
        {
            var res = await _projectService.Create(id, createProjectDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{projectId}"), AllowAnonymous]
        public async Task<IActionResult> UpdateProject(Guid id, Guid projectId, UpdateProjectDto updateProjectDto)
        {
            //if (updateProjectDto.LeaderId is null)
            //{
            //    var res = await _projectService.Update(userId: id, projectId, updateProjectDto);
            //    return CustomResult(res, HttpStatusCode.OK);
            //}
            //else
            //{
            //    var res = await _projectService.ChangeLeader(userId: id, projectId, updateProjectDto);
            //    return CustomResult(res, HttpStatusCode.OK);
            //}
            var res = await _projectService.Update(userId: id, projectId, updateProjectDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var result = await _projectService.Delete(projectId);
            return CustomResult(result, HttpStatusCode.OK);
        }

        /* [HttpGet("{projectId}"), AllowAnonymous]
         public async Task<IActionResult> Get(Guid projectId)
         {
             var result = await _projectService.Get(projectId);
             return CustomResult(result, HttpStatusCode.OK);
         }*/

        [HttpGet("{code}"), AllowAnonymous]
        public async Task<IActionResult> Get(string code)
        {
            var result = await _projectService.Get(code);
            if (result is null)
            {
                return CustomResult(result, HttpStatusCode.NoContent);
            }
            return CustomResult(result, HttpStatusCode.OK);
        }

        [HttpPost("members:add")]
        public async Task<IActionResult> AddMember([FromBody] AddMemberToProjectDto addMemberToProjectDto)
        {
            var res = await _projectService.AddMember(addMemberToProjectDto);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
