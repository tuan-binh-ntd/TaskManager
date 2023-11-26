﻿using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class PermissionGroupsController : BaseController
    {
        private readonly IPermissionGroupService _permissionGroupService;

        public PermissionGroupsController(IPermissionGroupService permissionGroupService)
        {
            _permissionGroupService = permissionGroupService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<PermissionGroupViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid projectId)
        {
            var res = await _permissionGroupService.GetPermissionGroupsByProjectId(projectId);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PermissionGroupViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreatePermissionGroupDto createPermissionGroupDto)
        {
            var res = await _permissionGroupService.Create(createPermissionGroupDto);

            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PermissionGroupViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionGroupDto updatePermissionGroupDto)
        {
            var res = await _permissionGroupService.Update(id, updatePermissionGroupDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _permissionGroupService.Delete(id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }


}