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
        }
    }
}
