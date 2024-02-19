namespace TaskManager.Application.Users.Commands.UploadPhoto;

public sealed class UserUploadPhotoCommand(
    Guid userId,
    IFormFile file
    )
    : ICommand<Result<UserViewModel>>
{
    public Guid UserId { get; private set; } = userId;
    public IFormFile File { get; private set; } = file;
}
