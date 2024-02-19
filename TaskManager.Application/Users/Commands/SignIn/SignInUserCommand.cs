namespace TaskManager.Application.Users.Commands.SignIn;

public sealed class SignInUserCommand(
    SignInDto signInDto
    )
     : ICommand<Result<UserViewModel>>
{
    public SignInDto SignInDto { get; private set; } = signInDto;
}
