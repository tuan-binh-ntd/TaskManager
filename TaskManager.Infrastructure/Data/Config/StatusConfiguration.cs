using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder
                .HasOne(s => s.StatusCategory)
                .WithMany(sc => sc.Statuses)
                .HasForeignKey(s => s.StatusCategoryId)
                .IsRequired();

            builder
                .HasOne(s => s.Project)
                .WithMany(sc => sc.Statuses)
                .HasForeignKey(s => s.ProjectId)
                .IsRequired(false);

            builder
                .HasMany(s => s.Issues)
                .WithOne(i => i.Status)
                .HasForeignKey(i => i.StatusId)
                .IsRequired(false);
        }
    }
}
