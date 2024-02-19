namespace TaskManager.Application.Users.Commands.ChangePassword;

public sealed class UserChangePasswordCommand(
    Guid userId,
    ChangePasswordDto changePasswordDto
    )
    : ICommand<Result<UserViewModel>>
{
    public Guid UserId { get; private set; } = userId;
    public ChangePasswordDto ChangePasswordDto { get; private set; } = changePasswordDto;
}
