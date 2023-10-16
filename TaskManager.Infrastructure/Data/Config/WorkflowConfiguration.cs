using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config
{
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
}
