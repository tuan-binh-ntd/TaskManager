namespace TaskManager.Application.Users.Commands.SignUp;

public sealed class SignUpUserCommand(
    SignUpDto signUpDto
    )
    : ICommand<Result<UserViewModel>>
{
    public SignUpDto SignUpDto { get; private set; } = signUpDto;
}
