namespace TaskManager.Infrastructure.Data.Config
{
    /*public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            builder
                .HasMany(pg => pg.PermissionRoles)
                .WithOne(pr => pr.PermissionGroup)
                .HasForeignKey(pr => pr.PermissionGroupId)
                .IsRequired();
        }
    }*/
}
