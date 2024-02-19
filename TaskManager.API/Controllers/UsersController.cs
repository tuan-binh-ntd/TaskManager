namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator)
    : ApiController(mediator)
{
    [HttpPost("signin"), AllowAnonymous]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(string))]
    public async Task<IActionResult> SignIn(SignInDto loginDto)
        => await Result.Success(new SignInUserCommand(loginDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPost("signup"), AllowAnonymous]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        => await Result.Success(new SignUpUserCommand(signUpDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}/change-password")]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(IEnumerable<IdentityError>))]
    public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordDto input)
        => await Result.Success(new UserChangePasswordCommand(id, input))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto input)
        => await Result.Success(new UpdateUserCommand(id, input))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPost("{id}/photos"), AllowAnonymous]
    [ProducesResponseType(typeof(UserViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile formFile)
        => await Result.Success(new UserUploadPhotoCommand(id, formFile))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}