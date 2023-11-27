using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Infrastructure.Data.Config
{
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
}
