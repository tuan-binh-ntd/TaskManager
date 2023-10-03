using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJWTTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public UserService(
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

        public async Task<object> SignIn(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(e => e.Email == loginDto.Email);

            if (user is null) return "Invalid email";

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return "Incorrect name or password";

            UserViewModel res = _mapper.Map<UserViewModel>(user);

            res.Token = await _jwtTokenService.CreateToken(user);

            return res;
        }

        public async Task<object> SignUp(SignUpDto signUpDto)
        {
            if (await CheckEmailExists(signUpDto.Email)) return "Email is taken";
            else if (await CheckUsernameExists(signUpDto.Username)) return "Username is taken";

            var user = _mapper.Map<AppUser>(signUpDto);
            user.UserName = signUpDto.Username;

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded) return result.Errors;

            UserViewModel res = new()
            {
                Name = user.UserName,
                Token = await _jwtTokenService.CreateToken(user)
            };
            return res;
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }

        public async Task<object?> ChangePassword(string id, PasswordDto passwordDto)
        {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user is null) return null;

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

            if (!result.Succeeded) return result.Errors;

            UserViewModel res = _mapper.Map<UserViewModel>(user);

            return res;
        }

        public async Task<IReadOnlyCollection<UserViewModel>> Gets()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Adapt<IReadOnlyCollection<UserViewModel>>();
        }
    }
}
