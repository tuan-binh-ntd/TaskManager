namespace TaskManager.Application.Users.Events;

internal sealed class UserRegisteredDomainEventHandler(
    ICriteriaRepository criteriaRepository,
    IStatusCategoryRepository statusCategoryRepository,
    IFilterRepository filterRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly ICriteriaRepository _criteriaRepository = criteriaRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository = statusCategoryRepository;
    private readonly IFilterRepository _filterRepository = filterRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var criterias = await _criteriaRepository.GetCriteriasAsync();

        var assigneeCriteria = criterias.Where(c => c.Name == CriteriaConstants.AssigneeCriteriaName).FirstOrDefault();
        var resolutionCriteria = criterias.Where(c => c.Name == CriteriaConstants.ResolutionCriteriaName).FirstOrDefault();
        var reporterCriteria = criterias.Where(c => c.Name == CriteriaConstants.ReporterCriteriaName).FirstOrDefault();
        var statusCategoryCriteria = criterias.Where(c => c.Name == CriteriaConstants.StatusCategoryCriteriaName).FirstOrDefault();
        var createdCriteria = criterias.Where(c => c.Name == CriteriaConstants.CreatedCriteriaName).FirstOrDefault();
        var resolvedCriteria = criterias.Where(c => c.Name == CriteriaConstants.ResolvedCriteriaName).FirstOrDefault();
        var updatedCriteria = criterias.Where(c => c.Name == CriteriaConstants.UpdatedCriteriaName).FirstOrDefault();
        var doneStatusCategory = await _statusCategoryRepository.GetDoneStatusCategoryAsync() ?? throw new StatusCategoryNullException();

        var filterConfiguration = new FilterConfiguration()
        {
            Assginee = new AssigneeCriteria()
            {
                CurrentUserId = notification.UserId,
            },
            Resolution = new ResolutionCriteria()
            {
                Unresolved = true,
                Done = false
            }
        };

        #region Create My open issues filter
        var myOpenIssues = new Filter()
        {
            Name = FilterConstants.MyOpenIssuesFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create Reported by me filter
        filterConfiguration = new FilterConfiguration()
        {
            Reporter = new ReporterCriteria()
            {
                CurrentUserId = notification.UserId,
            },
        };

        var reportedByMe = new Filter()
        {
            Name = FilterConstants.ReportedByMeFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create All issues filter
        var allIssues = new Filter()
        {
            Name = FilterConstants.AllIssuesFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = new FilterConfiguration().ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create open issues
        filterConfiguration = new FilterConfiguration()
        {
            Resolution = new ResolutionCriteria()
            {
                Unresolved = true,
                Done = false,
            },
        };

        var openIssues = new Filter()
        {
            Name = FilterConstants.OpenIssuesFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create done issues
        filterConfiguration = new FilterConfiguration()
        {
            StatusCategory = new StatusCategoryCriteria()
            {
                StatusCategoryIds = new List<Guid>()
                {
                    doneStatusCategory.Id
                }
            },
        };
        var doneIssues = new Filter()
        {
            Name = FilterConstants.DoneIssuesFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create Created recently filter
        filterConfiguration = new FilterConfiguration()
        {
            Created = new CreatedCriteria()
            {
                MoreThan = new MoreThan(1, FilterConstants.WeekUnit)
            },
        };

        var createdRecently = new Filter()
        {
            Name = FilterConstants.CreatedRecentlyFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create resolved recently filter
        filterConfiguration = new FilterConfiguration()
        {
            Resolved = new ResolvedCriteria()
            {
                MoreThan = new MoreThan(1, FilterConstants.WeekUnit)
            },
        };

        var resolvedRecently = new Filter()
        {
            Name = FilterConstants.ResolvedRecentlyFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        #region Create updated recently filter
        filterConfiguration = new FilterConfiguration()
        {
            Updated = new UpdatedCriteria()
            {
                MoreThan = new MoreThan(1, FilterConstants.WeekUnit)
            },
        };

        var updatedRecently = new Filter()
        {
            Name = FilterConstants.UpdatedRecentlyFilterName,
            Type = FilterConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = notification.UserId,
        };
        #endregion

        var filters = new List<Filter>
        {
            myOpenIssues, reportedByMe, allIssues, openIssues, doneIssues, createdRecently, resolvedRecently, updatedRecently
        };

        _filterRepository.InsertRange(filters);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
