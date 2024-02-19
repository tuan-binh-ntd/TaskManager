namespace TaskManager.Persistence.Data.Config;

public class VersionConfiguration : IEntityTypeConfiguration<Version>
{
    public void Configure(EntityTypeBuilder<Version> builder)
    {
        builder
            .ToTable("Versions");

        builder
            .HasMany(i => i.VersionIssues)
            .WithOne(v => v.Version)
            .HasForeignKey(i => i.VersionId)
            .IsRequired();
    }
}
