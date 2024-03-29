﻿namespace TaskManager.Persistence.Data.Config;

public class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder
            .HasMany(w => w.WorkflowIssueTypes)
            .WithOne(wi => wi.Workflow)
            .HasForeignKey(wi => wi.WorkflowId)
            .IsRequired();

        builder
            .HasOne(w => w.Project)
            .WithOne(p => p.Workflow)
            .IsRequired();
    }
}
