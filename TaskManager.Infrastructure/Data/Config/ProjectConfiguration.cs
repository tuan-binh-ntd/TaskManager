namespace TaskManager.Infrastructure.Data.Config;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        //Many to Many Relationship (AppUser, UserProject, Project)
        builder
            .HasMany(p => p.UserProjects)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .IsRequired();

        //Add Unique Constraint for Code Col
        builder
            .HasIndex(p => p.Code)
            .IsUnique();

        builder
            .HasOne(p => p.ProjectConfiguration)
            .WithOne(pg => pg.Project);

        builder
            .HasMany(p => p.Priorities)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .IsRequired();

        builder
            .HasMany(p => p.PermissionGroups)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .IsRequired();

        builder
           .HasMany(p => p.Labels)
           .WithOne(p => p.Project)
           .HasForeignKey(p => p.ProjectId)
           .IsRequired();
    }
}
