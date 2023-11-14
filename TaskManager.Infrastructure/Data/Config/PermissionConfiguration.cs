using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder
                .HasMany(pg => pg.PermissionRoles)
                .WithOne(pr => pr.Permission)
                .HasForeignKey(pr => pr.PermissionId)
                .IsRequired();
        }
    }
}
