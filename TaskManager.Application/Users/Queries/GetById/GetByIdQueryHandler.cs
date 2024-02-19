namespace TaskManager.Application.Users.Queries.GetById;

internal sealed class GetByIdQueryHandler(
    UserManager<AppUser> userManager
    )
     : IQueryHandler<GetByIdQuery, Result>
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task<Result> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return Result.Failure(UserDomainError.NotFound);
        return Result.Success(user.Adapt<UserViewModel>());
    }
}
