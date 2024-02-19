namespace TaskManager.Application.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler(
    UserManager<AppUser> userManager
    )
    : ICommandHandler<UpdateUserCommand, Result<UserViewModel>>
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task<Result<UserViewModel>> Handle(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(updateUserCommand.UserId.ToString());
        if (user is null) return Result.Failure<UserViewModel>(Error.NotFound);
        user.Name = !string.IsNullOrWhiteSpace(updateUserCommand.UpdateUserDto.Name) ? updateUserCommand.UpdateUserDto.Name : user.Name;
        user.Department = !string.IsNullOrWhiteSpace(updateUserCommand.UpdateUserDto.Department) ? updateUserCommand.UpdateUserDto.Department : user.Department;
        user.Organization = !string.IsNullOrWhiteSpace(updateUserCommand.UpdateUserDto.Organization) ? updateUserCommand.UpdateUserDto.Organization : user.Organization;
        user.AvatarUrl = !string.IsNullOrWhiteSpace(updateUserCommand.UpdateUserDto.AvatarUrl) ? updateUserCommand.UpdateUserDto.AvatarUrl : user.AvatarUrl;
        user.JobTitle = !string.IsNullOrWhiteSpace(updateUserCommand.UpdateUserDto.JobTitle) ? updateUserCommand.UpdateUserDto.JobTitle : user.JobTitle;
        user.Location = !string.IsNullOrWhiteSpace(updateUserCommand.UpdateUserDto.Location) ? updateUserCommand.UpdateUserDto.Location : user.Location;
        await _userManager.UpdateAsync(user);
        return Result.Success(user.Adapt<UserViewModel>());
    }
}
