namespace TaskManager.Persistence.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, Guid,
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options), IUnitOfWork, IDbContext
{
    private IDbContextTransaction _currentTransaction = null!;

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    #region IDbContext Implements
    public new DbSet<TEntity> Set<TEntity>()
            where TEntity : BaseEntity
            => base.Set<TEntity>();

    public async Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : BaseEntity
        => await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

    public void Insert<TEntity>(TEntity entity)
        where TEntity : BaseEntity
        => Set<TEntity>().Add(entity);

    public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : BaseEntity
        => Set<TEntity>().AddRange(entities);

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : BaseEntity
        => Set<TEntity>().Remove(entity);

    public void RemoveRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : BaseEntity
        => Set<TEntity>().RemoveRange(entities);

    public Task<int> ExecuteSqlAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken = default)
        => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    public DbSet<AppUser> AppUser => Users;
    #endregion

    #region DbSets
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Backlog> Backlogs => Set<Backlog>();
    public DbSet<UserProject> UserProjects => Set<UserProject>();
    public DbSet<Sprint> Sprints => Set<Sprint>();
    public DbSet<IssueType> IssueTypes => Set<IssueType>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<IssueHistory> IssueHistories => Set<IssueHistory>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<IssueDetail> IssueDetails => Set<IssueDetail>();
    public DbSet<ProjectConfiguration> ProjectConfigurations => Set<ProjectConfiguration>();
    public DbSet<StatusCategory> StatusCategories => Set<StatusCategory>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Transition> Transitions => Set<Transition>();
    public DbSet<Workflow> Workflows => Set<Workflow>();
    public DbSet<WorkflowIssueType> WorkflowIssueTypes => Set<WorkflowIssueType>();
    public DbSet<UserTeam> UserTeams => Set<UserTeam>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Priority> Priorities => Set<Priority>();
    public DbSet<Version> Versions => Set<Version>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationIssueEvent> NotificationIssueEvents => Set<NotificationIssueEvent>();
    public DbSet<IssueEvent> IssueEvents => Set<IssueEvent>();
    public DbSet<Filter> Filters => Set<Filter>();
    public DbSet<FilterCriteria> FilterCriterias => Set<FilterCriteria>();
    public DbSet<Criteria> Criterias => Set<Criteria>();
    public DbSet<PermissionGroup> PermissionGroups => Set<PermissionGroup>();
    public DbSet<VersionIssue> VersionIssues => Set<VersionIssue>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<LabelIssue> LabelIssues => Set<LabelIssue>();
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #region Transaction

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => Database.BeginTransactionAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction is null) return null!;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction is null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null!;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null!;
            }
        }
    }
    #endregion

    #region Override SaveChange
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IHasCreationTime hasCreationTime && entry.State == EntityState.Added)
            {
                hasCreationTime.CreationTime = DateTime.Now;
            }
            else if (entry.Entity is IHasModificationTime hasModificationTime && entry.State == EntityState.Modified)
            {
                hasModificationTime.ModificationTime = DateTime.Now;
            }
        }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IHasCreationTime hasCreationTime && entry.State == EntityState.Added)
            {
                hasCreationTime.CreationTime = DateTime.Now;
            }
            else if (entry.Entity is IHasModificationTime hasModificationTime && entry.State == EntityState.Modified)
            {
                hasModificationTime.ModificationTime = DateTime.Now;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
    #endregion
}
