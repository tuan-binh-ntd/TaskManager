using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config;

public class TransitionConfiguration : IEntityTypeConfiguration<Transition>
{
    public void Configure(EntityTypeBuilder<Transition> builder)
    {
        builder
            .HasOne(t => t.Project)
            .WithMany(p => p.Transitions)
            .HasForeignKey(t => t.ProjectId)
            .IsRequired(false);
    }
}
