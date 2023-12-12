using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config;

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
