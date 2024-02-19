namespace TaskManager.Application.Users.Commands.SignIn;

internal sealed class SignInUserCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJWTTokenService jWTTokenService
    )
    : ICommandHandler<SignInUserCommand, Result<UserViewModel>>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly IJWTTokenService _jWTTokenService = jWTTokenService;

    public async Task<Result<UserViewModel>> Handle(SignInUserCommand signInUserCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(e => e.Email == signInUserCommand.SignInDto.Email, cancellationToken: cancellationToken);

        if (user is null) return Result.Failure<UserViewModel>(UserDomainError.InvalidEmail);

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, signInUserCommand.SignInDto.Password, false);
        if (!result.Succeeded) return Result.Failure<UserViewModel>(UserDomainError.IncorrectEmailOrPassword);

        UserViewModel res = user.Adapt<UserViewModel>();

        res.Token = await _jWTTokenService.CreateToken(user);

        return Result.Success(res);
    }
}
