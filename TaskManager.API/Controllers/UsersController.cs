using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public UsersController(
            IUserService userService,
            IProjectService projectService
            )
        {
            _userService = userService;
            _projectService = projectService;
        }


        [AllowAnonymous]
        [HttpPost("signin")]
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
        public async Task<IActionResult> CheckEmail(string email)
        {
            var res = await _userService.CheckEmailExists(email);
            return CustomResult(new { Invalid = res }, HttpStatusCode.OK);
        }

        //[HttpGet("{username}"), AllowAnonymous]
        //public async Task<IActionResult> CheckUsername(string username)
        //{
        //    var res = await _userService.CheckUsernameExists(username);
        //    return CustomResult(new { Invalid = res }, HttpStatusCode.OK);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangePassword(string id, PasswordDto input)
        {
            var res = await _userService.ChangePassword(id, input);
            if (res is null)
            {
                return CustomResult("User isn't exists", HttpStatusCode.BadRequest);
            }
            else if (res is IEnumerable<IdentityError> errors)
            {
                return CustomResult(errors, HttpStatusCode.BadRequest);
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

        [HttpPost("{id}/projects")]
        public async Task<IActionResult> CreateProject(Guid id, ProjectDto projectDto)
        {
            var res = await _projectService.Create(id, projectDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}/projects/{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, ProjectDto projectDto)
        {
            var res = await _projectService.Update(projectId, projectDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}/projects/{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var result = await _projectService.Delete(projectId);
            return CustomResult(result, HttpStatusCode.OK);
        }
    }
}