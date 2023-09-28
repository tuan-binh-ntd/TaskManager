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

        public UsersController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJWTTokenService jwtTokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(e => e.Email == loginDto.Email);

            if (user is null) return CustomResult("Invalid username", HttpStatusCode.Unauthorized);

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return CustomResult("Incorrect name or password", HttpStatusCode.Unauthorized);

            UserViewModel res = _mapper.Map<UserViewModel>(user);

            res.Token = await _jwtTokenService.CreateToken(user);

            return CustomResult(res, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (await CheckUserExists(signUpDto.Email)) return CustomResult("Email is taken", HttpStatusCode.BadRequest);

            var user = _mapper.Map<AppUser>(signUpDto);
            user.UserName = signUpDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded) return CustomResult(result.Errors, HttpStatusCode.BadRequest);

            UserViewModel res = new()
            {
                Name = user.UserName,
                Token = await _jwtTokenService.CreateToken(user)
            };
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet("{username}"), AllowAnonymous]
        public async Task<IActionResult> CheckUsername(string username)
        {
            if (await CheckUserExists(username)) return CustomResult(new { Invalid = false }, HttpStatusCode.OK);
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

        private async Task<bool> CheckUserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }
    }
}