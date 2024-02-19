namespace TaskManager.Persistence.Data.Config;

public class BacklogConfiguration : IEntityTypeConfiguration<Backlog>
{
    public void Configure(EntityTypeBuilder<Backlog> builder)
    {
        builder
            .HasOne(b => b.Project)
            .WithOne(p => p.Backlog)
            .HasForeignKey<Backlog>(b => b.ProjectId)
            .IsRequired();
    }
}
