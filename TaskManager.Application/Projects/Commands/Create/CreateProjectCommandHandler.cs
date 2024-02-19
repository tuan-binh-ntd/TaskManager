namespace TaskManager.Application.Projects.Commands.Create;

internal sealed class CreateProjectCommandHandler(
    IProjectRepository projectRepository,
    IUserProjectRepository userProjectRepository,
    IPermissionGroupRepository permissionGroupRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateProjectCommand, Result<ProjectViewModel>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ProjectViewModel>> Handle(CreateProjectCommand createProjectCommand, CancellationToken cancellationToken)
    {
        var project = Project.Create(createProjectCommand.CreateProjectDto.Name,
            createProjectCommand.CreateProjectDto.Description,
            createProjectCommand.CreateProjectDto.Code,
            "https://bs-uploads.toptal.io/blackfish-uploads/components/skill_page/content/logo_file/logo/195649/JIRA_logo-e5a9c767df8a60eb2d242a356ce3fdca.jpg");
        project.ProjectCreated();
        _projectRepository.Insert(project);
        var projectLead = PermissionGroup.CreateProjectLeadRole(project.Id);
        _permissionGroupRepository.Insert(projectLead);
        UserProject userProject = UserProject.Create(RoleConstants.LeaderRole, projectLead.Id, false, project.Id, createProjectCommand.UserId);
        _userProjectRepository.Insert(userProject);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(project.Adapt<ProjectViewModel>());
    }
}
