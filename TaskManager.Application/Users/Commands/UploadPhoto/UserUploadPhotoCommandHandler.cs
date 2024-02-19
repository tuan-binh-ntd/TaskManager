namespace TaskManager.Application.Users.Commands.UploadPhoto;

internal sealed class UserUploadPhotoCommandHandler(
    UserManager<AppUser> userManager,
    IAzureStorageFileShareService azureStorageFileShareService
    )
    : ICommandHandler<UserUploadPhotoCommand, Result<UserViewModel>>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IAzureStorageFileShareService _azureStorageFileShareService = azureStorageFileShareService;

    public async Task<Result<UserViewModel>> Handle(UserUploadPhotoCommand userUploadPhotoCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userUploadPhotoCommand.UserId.ToString());
        if (user is null) return Result.Failure<UserViewModel>(Error.NotFound);
        string newAvatarUrl = await _azureStorageFileShareService.UploadPhotoForUserAsync(userUploadPhotoCommand.File);
        var symbolIndex = user.AvatarUrl.LastIndexOf('/');
        var fileName = user.AvatarUrl[(symbolIndex + 1)..];
        user.AvatarUrl = newAvatarUrl;
        await _azureStorageFileShareService.DeletePhotoForUserAsync(fileName);
        await _userManager.UpdateAsync(user);

        return Result.Success(user.Adapt<UserViewModel>());
    }
}
