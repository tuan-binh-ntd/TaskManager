namespace TaskManager.Application.Users.Commands.Update;

public sealed class UpdateUserCommand(
    Guid userId,
    UpdateUserDto updateUserDto
    )
    : ICommand<Result<UserViewModel>>
{
    public Guid UserId { get; private set; } = userId;
    public UpdateUserDto UpdateUserDto { get; private set; } = updateUserDto;
}
