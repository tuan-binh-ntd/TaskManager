﻿using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<object> SignIn(LoginDto loginDto);
        Task<object> SignUp(SignUpDto signUpDto);
        Task<bool> CheckEmailExists(string email);
        Task<bool> CheckUsernameExists(string username);
        Task<object?> ChangePassword(string id, PasswordDto passwordDto);
        Task<IReadOnlyCollection<UserViewModel>> Gets();
    }
}