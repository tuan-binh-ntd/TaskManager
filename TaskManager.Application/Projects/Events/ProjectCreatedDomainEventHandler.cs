using TaskManager.Core.Constants;

namespace TaskManager.Application.Projects.Events;

internal sealed class ProjectCreatedDomainEventHandler(
    IPriorityRepository priorityRepository,
    IBacklogRepository backlogRepository,
    IProjectConfigurationRepository projectConfigurationRepository,
    IStatusCategoryRepository statusCategoryRepository,
    IStatusRepository statusRepository,
    IIssueTypeRepository issueTypeRepository,
    IPermissionGroupRepository permissionGroupRepository,
    IIssueEventRepository issueEventRepository,
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<ProjectCreatedDomainEvent>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly IProjectConfigurationRepository _projectConfigurationRepository = projectConfigurationRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository = statusCategoryRepository;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IPermissionGroupRepository _permissionGroupRepository = permissionGroupRepository;
    private readonly IIssueEventRepository _issueEventRepository = issueEventRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(ProjectCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Define default priority for project
        var mediumPriority = Priority.CreateMediumPriority(notification.Project.Id);
        _priorityRepository.Insert(Priority.CreateLowestPriority(notification.Project.Id));
        _priorityRepository.Insert(Priority.CreateLowPriority(notification.Project.Id));
        _priorityRepository.Insert(mediumPriority);
        _priorityRepository.Insert(Priority.CreateHighPriority(notification.Project.Id));
        _priorityRepository.Insert(Priority.CreateHighestPriority(notification.Project.Id));

        // Define backlog for project
        _backlogRepository.Insert(Backlog.Create(notification.Project.Name, notification.Project.Id));

        // Define project configuration for project
        _projectConfigurationRepository.Insert(ProjectConfiguration.Create(notification.Project.Id, 0, 0, notification.Project.Code, null, mediumPriority.Id));

        // Define default status for project
        var statusCategories = await _statusCategoryRepository.GetStatusCategorysAsync();

        var todoId = statusCategories.Where(e => e.Code == StatusCategoryConstants.ToDoCode).Select(e => e.Id).FirstOrDefault();
        var inProgressId = statusCategories.Where(e => e.Code == StatusCategoryConstants.InProgressCode).Select(e => e.Id).FirstOrDefault();
        var doneId = statusCategories.Where(e => e.Code == StatusCategoryConstants.DoneCode).Select(e => e.Id).FirstOrDefault();
        var versionId = statusCategories.Where(e => e.Code == StatusCategoryConstants.VersionCode).Select(e => e.Id).FirstOrDefault();

        _statusRepository.Insert(Status.Create(StatusConstants.TodoStatusName, string.Empty, 1, true, true, notification.Project.Id, todoId));
        _statusRepository.Insert(Status.Create(StatusConstants.InProgresstatusName, string.Empty, 2, true, true, notification.Project.Id, inProgressId));
        _statusRepository.Insert(Status.Create(StatusConstants.DoneStatusName, string.Empty, 3, true, true, notification.Project.Id, doneId));

        _statusRepository.Insert(Status.Create(StatusConstants.UnreleasedStatusName, string.Empty, 0, true, false, notification.Project.Id, versionId));
        _statusRepository.Insert(Status.Create(StatusConstants.ReleasedStatusName, string.Empty, 0, true, false, notification.Project.Id, versionId));
        _statusRepository.Insert(Status.Create(StatusConstants.ArchivedStatusName, string.Empty, 0, true, false, notification.Project.Id, versionId));

        // Define default issue type for project
        _issueTypeRepository.Insert(IssueType.CreateEpicType(notification.Project.Id));
        _issueTypeRepository.Insert(IssueType.CreateStoryType(notification.Project.Id));
        _issueTypeRepository.Insert(IssueType.CreateBugType(notification.Project.Id));
        _issueTypeRepository.Insert(IssueType.CreateTaskType(notification.Project.Id));
        _issueTypeRepository.Insert(IssueType.CreateSubtaskType(notification.Project.Id));

        // Define default permission group for project
        _permissionGroupRepository.Insert(PermissionGroup.CreateProductOwnerRole(notification.Project.Id));
        _permissionGroupRepository.Insert(PermissionGroup.CreateScrumMasterRole(notification.Project.Id));
        _permissionGroupRepository.Insert(PermissionGroup.CreateDeveloperRole(notification.Project.Id));

        var issueEvents = await _issueEventRepository.GetIssueEventsAsync();
        var notificationSchema = Notification.Create(NotificationConstants.NotificationName,
            NotificationConstants.NotificationDesc,
            [],
            notification.Project.Id);

        foreach (var item in issueEvents)
        {
            var notificationIssueEvent = NotificationIssueEvent.Create(true, true, true, false, notificationSchema.Id, item.Id);
            notificationSchema.NotificationIssueEvents!.Add(notificationIssueEvent);
        }
        _notificationRepository.Insert(notificationSchema);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
