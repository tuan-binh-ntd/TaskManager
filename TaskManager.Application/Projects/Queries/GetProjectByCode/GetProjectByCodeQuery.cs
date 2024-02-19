namespace TaskManager.Application.Projects.Queries.GetProjectByCode;

public sealed class GetProjectByCodeQuery(
    Guid UserId,
    string code
    )
    : IQuery<Maybe<ProjectViewModel>>
{
    public Guid UserId { get; private set; } = UserId;
    public string Code { get; private set; } = code;
}
