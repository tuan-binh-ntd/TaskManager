using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(
            IRoleService roleService
            )
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var res = await _roleService.Gets();
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var res = await _roleService.Get(id);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppRoleDto appRoleDto)
        {
            var res = await _roleService.Create(appRoleDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateAppRoleDto appRoleDto)
        {
            var res = await _roleService.Update(id, appRoleDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _roleService.Delete(id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
