namespace TaskManager.Core.Interfaces.Services;

public interface IUserService
{
    Task<object> SignIn(LoginDto loginDto);
    Task<object> SignUp(SignUpDto signUpDto);
    Task<bool> CheckEmailExists(string email);
    Task<bool> CheckUsernameExists(string username);
    Task<object?> ChangePassword(string id, PasswordDto passwordDto);
    Task<IReadOnlyCollection<UserViewModel>> Gets(GetUserByFilterDto filter);
    Task<UserViewModel?> GetById(Guid id);
    Task<UserViewModel> Update(Guid id, UpdateUserDto updateUserDto);
    Task<UserViewModel> UploadPhoto(Guid id, IFormFile file);
}
