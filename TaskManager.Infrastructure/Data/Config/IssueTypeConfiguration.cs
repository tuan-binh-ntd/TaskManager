using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config
{
    public class IssueTypeConfiguration : IEntityTypeConfiguration<IssueType>
    {
        public void Configure(EntityTypeBuilder<IssueType> builder)
        {
            builder
                .HasOne(it => it.Project)
                .WithMany(p => p.IssueTypes)
                .HasForeignKey(it => it.ProjectId)
                .IsRequired(false);

            builder
               .HasMany(w => w.WorkflowIssueTypes)
               .WithOne(wi => wi.IssueType)
               .HasForeignKey(wi => wi.IssueTypeId)
               .IsRequired();
        }
    }
}
