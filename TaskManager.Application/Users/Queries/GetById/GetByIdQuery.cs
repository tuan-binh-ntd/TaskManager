namespace TaskManager.Application.Users.Queries.GetById;

internal sealed class GetByIdQuery(
    Guid userId
    )
    : IQuery<Result>
{
    public Guid UserId { get; private set; } = userId;
}
