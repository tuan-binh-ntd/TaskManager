namespace TaskManager.Application.Sprints.Queries.GetById;

public sealed class GetSprintByIdQuery(
    Guid sprintId
    )
    : IQuery<Maybe<SprintViewModel>>
{
    public Guid SprintId { get; private set; } = sprintId;
}
