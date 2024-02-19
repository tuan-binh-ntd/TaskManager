using TaskManager.Application.Sprints.Commands.Create;

namespace TaskManager.Application.Sprints.Commands.Complete;

internal sealed class CompleteSprintCommandHandler(
    ISprintRepository sprintRepository,
    IIssueRepository issueRepository,
    IBacklogRepository backlogRepository,
    IMediator mediator,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CompleteSprintCommand, Result<SprintViewModel>>
{
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly IMediator _mediator = mediator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SprintViewModel>> Handle(CompleteSprintCommand completeSprintCommand, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(completeSprintCommand.SprintId);
        if (sprint is null) return Result.Failure<SprintViewModel>(Error.NotFound);
        var issues = await _issueRepository.GetNotDoneIssuesBySprintIdAsync(completeSprintCommand.SprintId, completeSprintCommand.ProjectId);

        if (completeSprintCommand.CompleteSprintDto.Option == SprintConstants.NewSprintOption)
        {
            var newSprint = (await _mediator.Send(new CreateSprintCommand(completeSprintCommand.ProjectId), cancellationToken)).Value;

            foreach (var issue in issues)
            {
                issue.SprintId = newSprint.Id;
            }
            _issueRepository.UpdateRange(issues);
        }
        else if (completeSprintCommand.CompleteSprintDto.Option == SprintConstants.BacklogOption)
        {
            var backlog = await _backlogRepository.GetBacklogByProjectIdAsync(completeSprintCommand.ProjectId);
            if (backlog is null) return Result.Failure<SprintViewModel>(Error.NotFound);
            foreach (var issue in issues)
            {
                issue.SprintId = null;
                issue.BacklogId = backlog.Id;
            }
            _issueRepository.UpdateRange(issues);
        }
        else
        {
            if (completeSprintCommand.CompleteSprintDto.SprintId is Guid specificSprintId)
            {
                var specificSprint = await _sprintRepository.GetByIdAsync(specificSprintId);

                if (specificSprint is null) return Result.Failure<SprintViewModel>(Error.NotFound);

                foreach (var issue in issues)
                {
                    issue.SprintId = specificSprint.Id;
                }
                _issueRepository.UpdateRange(issues);
            }
        }

        sprint.Complete();
        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(sprint.Adapt<SprintViewModel>());
    }
}
