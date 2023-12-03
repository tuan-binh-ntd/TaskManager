using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class PermissionsController : BaseController
    //{
    //    private readonly IPermissionService _permissionService;
    //    public PermissionsController(IPermissionService permissionService)
    //    {
    //        _permissionService = permissionService;
    //    }

    //    [HttpGet]
    //    [ProducesResponseType(typeof(IReadOnlyCollection<PermissionViewModel>), (int)HttpStatusCode.OK)]
    //    public async Task<IActionResult> Get()
    //    {
    //        var res = await _permissionService.GetPermissionViewModels();
    //        return CustomResult(res, HttpStatusCode.OK);
    //    }

    //    [HttpPost]
    //    [ProducesResponseType(typeof(PermissionViewModel), (int)HttpStatusCode.Created)]
    //    public async Task<IActionResult> Create([FromBody] CreatePermissionDto createPermissionDto)
    //    {
    //        var res = await _permissionService.CreatePermission(createPermissionDto);
    //        return CustomResult(res, HttpStatusCode.Created);
    //    }

    //    [HttpPut("{id}")]
    //    [ProducesResponseType(typeof(PermissionViewModel), (int)HttpStatusCode.OK)]
    //    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePermissionDto updatePermissionDto)
    //    {
    //        var res = await _permissionService.UpdatePermission(id, updatePermissionDto);
    //        return CustomResult(res, HttpStatusCode.OK);
    //    }

    //    [HttpDelete("{id}")]
    //    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    //    public async Task<IActionResult> Delete(Guid id)
    //    {
    //        var res = await _permissionService.DeletePermission(id);
    //        return CustomResult(res, HttpStatusCode.OK);
    //    }
    //}
}
