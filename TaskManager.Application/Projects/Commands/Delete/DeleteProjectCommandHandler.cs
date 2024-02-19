namespace TaskManager.Application.Projects.Commands.Delete;

internal sealed class DeleteProjectCommandHandler(
    IProjectRepository projectRepository,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteProjectCommand, Result<Guid>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteProjectCommand deleteProjectCommand, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(deleteProjectCommand.ProjectId);

        if (project is null) return Result.Failure<Guid>(Error.NotFound);

        await _projectRepository.LoadIssueTypesAsync(project);
        await _projectRepository.LoadStatusesAsync(project);
        await _projectRepository.LoadBacklogAsync(project);
        await _projectRepository.LoadUserProjectsAsync(project);
        await _projectRepository.LoadProjectConfigurationAsync(project);
        await _projectRepository.LoadTransitionAsync(project);
        await _projectRepository.LoadWorkflowAsync(project);
        await _projectRepository.LoadPrioritiesAsync(project);
        await _projectRepository.LoadPermissionGroupsAsync(project);
        await _projectRepository.LoadSprintsAsync(project);
        await _projectRepository.LoadVersionsAsync(project);

        await _issueRepository.DeleteByProjectIdAsync(project.Id);
        if (project.Sprints is not null && project.Sprints.Count != 0)
        {
            foreach (var sprint in project.Sprints)
            {
                await _issueRepository.DeleteBySprintIdAsync(sprint.Id);
            }
        }
        if (project.Backlog is not null)
        {
            await _issueRepository.DeleteByBacklogIdAsync(project.Backlog.Id);
        }

        _projectRepository.Remove(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(deleteProjectCommand.ProjectId);
    }
}
