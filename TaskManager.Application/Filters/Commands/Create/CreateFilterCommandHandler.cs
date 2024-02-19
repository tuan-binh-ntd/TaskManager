namespace TaskManager.Application.Filters.Commands.Create;

internal sealed class CreateFilterCommandHandler(
    IFilterRepository filterRepository,
    ISprintRepository sprintRepository,
    IBacklogRepository backlogRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateFilterCommand, Result<FilterViewModel>>
{
    private readonly IFilterRepository _filterRepository = filterRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<FilterViewModel>> Handle(CreateFilterCommand createFilterCommand, CancellationToken cancellationToken)
    {
        if (createFilterCommand.CreateFilterDto.Project is not null
            && createFilterCommand.CreateFilterDto.Project.ProjectIds is not null
            && createFilterCommand.CreateFilterDto.Project.ProjectIds.Count != 0)
        {
            createFilterCommand.CreateFilterDto.Project.BacklogIds = await _backlogRepository
                .GetBacklogIdsByProjectIdsAsync(createFilterCommand.CreateFilterDto.Project.ProjectIds);

            createFilterCommand.CreateFilterDto.Project.SprintIds = await _sprintRepository
                .GetSprintIdsByProjectIdsAsync(createFilterCommand.CreateFilterDto.Project.ProjectIds);
        }
        var filterConfiguration = new FilterConfiguration()
        {
            Project = createFilterCommand.CreateFilterDto.Project,
            Type = createFilterCommand.CreateFilterDto.Type,
            Status = createFilterCommand.CreateFilterDto.Status,
            Assginee = createFilterCommand.CreateFilterDto.Assginee,
            Created = createFilterCommand.CreateFilterDto.Created,
            DueDate = createFilterCommand.CreateFilterDto.DueDate,
            FixVersions = createFilterCommand.CreateFilterDto.FixVersions,
            Labels = createFilterCommand.CreateFilterDto.Labels,
            Priority = createFilterCommand.CreateFilterDto.Priority,
            Reporter = createFilterCommand.CreateFilterDto.Reporter,
            Resolution = createFilterCommand.CreateFilterDto.Resolution,
            Resolved = createFilterCommand.CreateFilterDto.Resolved,
            Sprints = createFilterCommand.CreateFilterDto.Sprints,
            StatusCategory = createFilterCommand.CreateFilterDto.StatusCategory,
            Updated = createFilterCommand.CreateFilterDto.Updated,
        };

        var filter = new Filter()
        {
            Name = createFilterCommand.CreateFilterDto.Name,
            Stared = createFilterCommand.CreateFilterDto.Stared,
            Type = createFilterCommand.CreateFilterDto.Stared ? FilterConstants.StaredFiltersType : FilterConstants.CreatedUserFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = createFilterCommand.CreateFilterDto.CreatorUserId,
            Descrption = createFilterCommand.CreateFilterDto.Descrption,
        };

        _filterRepository.Insert(filter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new FilterViewModel()
        {
            Id = filter.Id,
            Name = filter.Name,
            Stared = filter.Stared,
            Type = filter.Type,
            Configuration = filter.Configuration.FromJson<FilterConfiguration>(),
            Description = filter.Descrption
        });
    }
}
