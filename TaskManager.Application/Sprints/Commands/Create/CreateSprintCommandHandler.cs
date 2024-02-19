namespace TaskManager.Application.Sprints.Commands.Create;

internal sealed class CreateSprintCommandHandler(
    IProjectConfigurationRepository projectConfigurationRepository,
    ISprintRepository sprintRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateSprintCommand, Result<SprintViewModel>>
{
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SprintViewModel>> Handle(CreateSprintCommand createSprintCommand, CancellationToken cancellationToken)
    {
        var projectConfiguration = await _projectConfigurationRepository.GetProjectConfigurationByProjectIdAsync(createSprintCommand.ProjectId);
        if (projectConfiguration is null) return Result.Failure<SprintViewModel>(Error.NotFound);
        int sprintIndex = projectConfiguration.SprintCode + 1;

        Sprint sprint = Sprint.Create($"{projectConfiguration.Code} Sprint {sprintIndex}",
            null,
            null,
            string.Empty,
            createSprintCommand.ProjectId);

        _sprintRepository.Insert(sprint);
        projectConfiguration.SprintCode = sprintIndex;
        _projectConfigurationRepository.Update(projectConfiguration);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(sprint.Adapt<SprintViewModel>());
    }
}
