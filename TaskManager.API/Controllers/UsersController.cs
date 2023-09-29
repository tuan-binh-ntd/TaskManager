using AutoMapper;
using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Infrastructure.DTOs;
using TaskManager.Infrastructure.ViewModel;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJWTTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public UsersController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJWTTokenService jwtTokenService,
            IMapper mapper,
            IProjectService projectService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            _projectService = projectService;
        }


        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(e => e.Email == loginDto.Email);

            if (user is null) return CustomResult("Invalid email", HttpStatusCode.Unauthorized);

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return CustomResult("Incorrect name or password", HttpStatusCode.Unauthorized);

            UserViewModel res = _mapper.Map<UserViewModel>(user);

            res.Token = await _jwtTokenService.CreateToken(user);

            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost("signup"), AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (await CheckEmailExists(signUpDto.Email)) return CustomResult("Email is taken", HttpStatusCode.BadRequest);
            else if (await CheckUserNameExists(signUpDto.Username)) return CustomResult("Username is taken", HttpStatusCode.BadRequest);

            var user = _mapper.Map<AppUser>(signUpDto);
            user.UserName = signUpDto.Username;

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded) return CustomResult(result.Errors, HttpStatusCode.BadRequest);

            UserViewModel res = new()
            {
                Name = user.UserName,
                Token = await _jwtTokenService.CreateToken(user)
            };
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet("{email}"), AllowAnonymous]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (await CheckEmailExists(email)) return CustomResult(new { Invalid = false }, HttpStatusCode.OK);
            return CustomResult(new { Invalid = true }, HttpStatusCode.OK);
        }

        [HttpGet("{username}"), AllowAnonymous]
        public async Task<IActionResult> CheckUsername(string username)
        {
            if (await CheckUserNameExists(username)) return CustomResult(new { Invalid = false }, HttpStatusCode.OK);
            return CustomResult(new { Invalid = true }, HttpStatusCode.OK);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangePassword(string id, PasswordDto input)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user is null) return CustomResult(HttpStatusCode.NoContent);

            var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);

            if (!result.Succeeded) return CustomResult("Password incorrect", result.Errors, HttpStatusCode.BadRequest);

            UserViewModel res = _mapper.Map<UserViewModel>(user);

            return CustomResult(res, HttpStatusCode.OK);
        }

        private async Task<bool> CheckEmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<bool> CheckUserNameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }

        [HttpPost("{id}/projects")]
        public async Task<IActionResult> CreateProject(Guid id, ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            project.LeaderId = id;
            var result = await _projectService.CreateProject(project);
            return CustomResult(_mapper.Map<ProjectViewModel>(result), HttpStatusCode.Created);
        }
    }
}