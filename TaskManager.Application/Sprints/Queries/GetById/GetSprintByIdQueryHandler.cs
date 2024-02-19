namespace TaskManager.Application.Sprints.Queries.GetById;

internal sealed class GetSprintByIdQueryHandler(
    ISprintRepository sprintRepository
    )
    : IQueryHandler<GetSprintByIdQuery, Maybe<SprintViewModel>>
{
    private readonly ISprintRepository _sprintRepository = sprintRepository;

    public async Task<Maybe<SprintViewModel>> Handle(GetSprintByIdQuery request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId);
        if (sprint is null) return Maybe<SprintViewModel>.None;
        return Maybe<SprintViewModel>.From(sprint.Adapt<SprintViewModel>());
    }
}
