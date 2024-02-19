namespace TaskManager.Application.Users.Commands.SignUp;

internal sealed class SignUpUserCommandHandler(
    UserManager<AppUser> userManager,
    ITextToImageService textToImageService,
    IJWTTokenService jWTTokenService
    )
    : ICommandHandler<SignUpUserCommand, Result<UserViewModel>>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITextToImageService _textToImageService = textToImageService;
    private readonly IJWTTokenService _jWTTokenService = jWTTokenService;

    public async Task<Result<UserViewModel>> Handle(SignUpUserCommand signUpUserCommand, CancellationToken cancellationToken)
    {
        if (await CheckEmailExists(signUpUserCommand.SignUpDto.Email)) Result.Failure<UserViewModel>(UserDomainError.EmailTaken);

        var avatarUrl = await _textToImageService.GenerateImageAsync(signUpUserCommand.SignUpDto.Name);

        var user = AppUser.Create(signUpUserCommand.SignUpDto.Name,
            string.Empty,
            string.Empty,
            avatarUrl,
            string.Empty,
            string.Empty,
           signUpUserCommand.SignUpDto.Email);

        _ = await _userManager.CreateAsync(user, signUpUserCommand.SignUpDto.Password);

        //if (!result.Succeeded) return Result.Failure(result.Errors);

        UserViewModel res = new()
        {
            Token = await _jWTTokenService.CreateToken(user),
            Id = user.Id,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Department = user.Department,
            Organization = user.Organization,
            JobTitle = user.JobTitle,
            Location = user.Location,
            Name = user.Name,
        };

        return Result.Success(res);
    }

    private async Task<bool> CheckEmailExists(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }
}
