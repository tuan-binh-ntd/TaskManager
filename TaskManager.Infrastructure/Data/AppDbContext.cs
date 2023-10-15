﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;

namespace TaskManager.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid,
        IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction = null!;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;


        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region Transaction
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
}
