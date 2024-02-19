namespace TaskManager.Application.Filters.Queries.GetFilterViewModelsByUserId;

public sealed class GetFilterViewModelsByUserIdQuery(
    Guid userId
    )
    : IQuery<Maybe<IReadOnlyCollection<FilterViewModel>>>
{
    public Guid UserId { get; private set; } = userId;
}
