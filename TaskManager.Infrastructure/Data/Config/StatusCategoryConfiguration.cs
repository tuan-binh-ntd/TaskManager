using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config;

public class StatusCategoryConfiguration : IEntityTypeConfiguration<StatusCategory>
{
    public void Configure(EntityTypeBuilder<StatusCategory> builder)
    {
        builder
            .HasIndex(sc => sc.Code)
            .IsUnique();
    }
}
