namespace TaskManager.Persistence.Data.Config;

public class StatusCategoryConfiguration : IEntityTypeConfiguration<StatusCategory>
{
    public void Configure(EntityTypeBuilder<StatusCategory> builder)
    {
        builder
            .HasIndex(sc => sc.Code)
            .IsUnique();
    }
}
