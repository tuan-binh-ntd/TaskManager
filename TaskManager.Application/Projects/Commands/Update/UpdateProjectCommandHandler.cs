namespace TaskManager.Application.Projects.Commands.Update;

internal sealed class UpdateProjectCommandHandler(
    IProjectRepository projectRepository,
    IUserProjectRepository userProjectRepository,
    IProjectConfigurationRepository projectConfigurationRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateProjectCommand, Result<ProjectViewModel>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ProjectViewModel>> Handle(UpdateProjectCommand updateProjectCommand, CancellationToken cancellationToken)
    {
        Project? project = await _projectRepository.GetByIdAsync(updateProjectCommand.ProjectId);

        if (project is null) return Result.Failure<ProjectViewModel>(Error.NotFound);

        project = updateProjectCommand.UpdateProjectDto.Adapt(project);

        if (updateProjectCommand.UpdateProjectDto.IsFavourite is bool isFavourite)
        {
            await _userProjectRepository.UpdateIsFavouriteColAsync(project.Id, updateProjectCommand.UserId, isFavourite);
        }

        var projectConfiguration = await _projectConfigurationRepository.GetProjectConfigurationByProjectIdAsync(project.Id);

        if (projectConfiguration is null) return Result.Failure<ProjectViewModel>(Error.NotFound);

        projectConfiguration.DefaultAssigneeId = updateProjectCommand.UpdateProjectDto.DefaultAssigneeId;
        projectConfiguration.DefaultPriorityId = updateProjectCommand.UpdateProjectDto.DefaultPriorityId;

        _projectConfigurationRepository.Update(projectConfiguration);
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(project.Adapt<ProjectViewModel>());
    }
}
