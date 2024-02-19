namespace TaskManager.Persistence.Data.Config;

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
