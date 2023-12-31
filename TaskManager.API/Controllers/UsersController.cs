using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(
        IUserService userService
        )
    {
        _userService = userService;
    }


    [HttpPost("signin"), AllowAnonymous]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> SignIn(LoginDto loginDto)
    {
        var res = await _userService.SignIn(loginDto);
        if (res is string errorTxt)
        {
            return CustomResult(errorTxt, HttpStatusCode.BadRequest);
        }
        else if (res is UserViewModel userViewModel)
        {
            return CustomResult(userViewModel, HttpStatusCode.OK);
        }
        else
        {
            return CustomResult(HttpStatusCode.NoContent);
        }
    }

    [HttpPost("signup"), AllowAnonymous]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        var res = await _userService.SignUp(signUpDto);
        if (res is string errorTxt)
        {
            return CustomResult(errorTxt, HttpStatusCode.BadRequest);
        }
        else if (res is UserViewModel userViewModel)
        {
            return CustomResult(userViewModel, HttpStatusCode.OK);
        }
        else if (res is IEnumerable<IdentityError> errors)
        {
            return CustomResult(errors, HttpStatusCode.BadRequest);
        }
        else
        {
            return CustomResult(HttpStatusCode.NoContent);
        }
    }

    [HttpGet("{email}"), AllowAnonymous]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CheckEmail(string email)
    {
        var res = await _userService.CheckEmailExists(email);
        return CustomResult(new { Invalid = res }, HttpStatusCode.OK);
    }

    [HttpPut("{id}/change-password")]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> ChangePassword(string id, PasswordDto input)
    {
        var res = await _userService.ChangePassword(id, input);
        if (res is null)
        {
            return CustomResult("User isn't exists", HttpStatusCode.BadRequest);
        }
        else if (res is IEnumerable<IdentityError> errors)
        {
            return CustomResult("Password incorrect", errors, HttpStatusCode.BadRequest);
        }
        else if (res is UserViewModel userViewModel)
        {
            return CustomResult(userViewModel, HttpStatusCode.OK);
        }
        else
        {
            return CustomResult(HttpStatusCode.NoContent);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto input)
    {
        var res = await _userService.Update(id, input);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await _userService.GetById(id);
        if (res is null) return CustomResult(res, HttpStatusCode.NotFound);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<UserViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets([FromQuery] GetUserByFilterDto filter)
    {
        var res = await _userService.Gets(filter);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost("{id}/photos"), AllowAnonymous]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile formFile)
    {
        var res = await _userService.UploadPhoto(id, formFile);
        return CustomResult(res, HttpStatusCode.OK);
    }
}