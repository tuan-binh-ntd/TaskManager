using TaskManager.Persistence.Repositories;

namespace TaskManager.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

        // Repositories
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        services.AddScoped<IBacklogRepository, BacklogRepository>();

        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<ICriteriaRepository, CriteriaRepository>();

        services.AddScoped<IDashboardRepository, DashboardRepository>();

        services.AddScoped<IFilterRepository, FilterRepository>();

        services.AddScoped<IIssueDetailRepository, IssueDetailRepository>();

        services.AddScoped<IIssueEventRepository, IssueEventRepository>();

        services.AddScoped<IIssueHistoryRepository, IssueHistoryRepository>();

        services.AddScoped<IIssueRepository, IssueRepository>();

        services.AddScoped<IIssueTypeRepository, IssueTypeRepository>();

        services.AddScoped<ILabelIssueRepository, LabelIssueRepository>();

        services.AddScoped<ILabelRepository, LabelRepository>();

        services.AddScoped<INotificationIssueEventRepository, NotificationIssueEventRepository>();

        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();

        services.AddScoped<IPriorityRepository, PriorityRepository>();

        services.AddScoped<IProjectConfigurationRepository, ProjectConfigurationRepository>();

        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddScoped<ISprintRepository, SprintRepository>();

        services.AddScoped<IConnectionFactory, SqlConnectionFactory>();

        services.AddScoped<IStatusCategoryRepository, StatusCategoryRepository>();

        services.AddScoped<IStatusRepository, StatusRepository>();

        services.AddScoped<ITeamRepository, TeamRepository>();

        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();

        services.AddScoped<IUserProjectRepository, UserProjectRepository>();

        services.AddScoped<IVersionIssueRepository, VersionIssueRepository>();

        services.AddScoped<IVersionRepository, VersionRepository>();

        services.AddScoped<IWorkflowIssueTypeRepository, WorkflowIssueTypeRepository>();

        services.AddScoped<IWorkflowRepository, WorkflowRepository>();

        return services;
    }
}
