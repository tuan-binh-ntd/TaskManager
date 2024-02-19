namespace TaskManager.Application.Users.Commands.ChangePassword;

internal sealed class UserChangePasswordCommandHandler(
    UserManager<AppUser> userManager
    )
    : ICommandHandler<UserChangePasswordCommand, Result<UserViewModel>>
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task<Result<UserViewModel>> Handle(UserChangePasswordCommand userChangePasswordCommand, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.FindByIdAsync(userChangePasswordCommand.UserId.ToString());
        if (user is null) return Result.Failure<UserViewModel>(UserDomainError.NotFound);

        var result = await _userManager.ChangePasswordAsync(user,
            userChangePasswordCommand.ChangePasswordDto.CurrentPassword,
            userChangePasswordCommand.ChangePasswordDto.NewPassword);

        if (!result.Succeeded) return Result.Failure<UserViewModel>(UserDomainError.ChangePasswordFailure);

        return Result.Success(user.Adapt<UserViewModel>());
    }
}
