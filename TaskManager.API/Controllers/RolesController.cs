using Ardalis.GuardClauses;
using AutoMapper;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TaskManager.Core.Entities;
using TaskManager.Infrastructure.DTOs;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(
            IMapper mapper,
            RoleManager<AppRole> roleManager
            )
        {
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return CustomResult(roles, HttpStatusCode.OK);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return CustomResult(role, HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRoleDto appRoleDto)
        {
            AppRole appRole = _mapper.Map<AppRole>(appRoleDto);
            var role = await _roleManager.CreateAsync(appRole);
            return CustomResult(role, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AppRoleDto appRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            Guard.Against.Null(role, nameof(role));
            role = _mapper.Map<AppRole>(appRoleDto);
            await _roleManager.UpdateAsync(role);
            return CustomResult(role, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            Guard.Against.Null(role, nameof(role));

            await _roleManager.DeleteAsync(role);

            return CustomResult(role, HttpStatusCode.OK);
        }
    }
}
