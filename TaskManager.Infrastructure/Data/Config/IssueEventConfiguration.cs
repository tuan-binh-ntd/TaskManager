using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config;

public class IssueEventConfiguration : IEntityTypeConfiguration<IssueEvent>
{
    public void Configure(EntityTypeBuilder<IssueEvent> builder)
    {
        builder
            .HasMany(ie => ie.NotificationIssueEvents)
            .WithOne(ie => ie.IssueEvent)
            .HasForeignKey(ie => ie.IssueEventId)
            .IsRequired();
    }
}
