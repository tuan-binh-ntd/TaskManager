namespace TaskManager.Infrastructure.Data.Config;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .HasMany(ie => ie.NotificationIssueEvents)
            .WithOne(ie => ie.Notification)
            .HasForeignKey(ie => ie.NotificationId)
            .IsRequired();
    }
}
