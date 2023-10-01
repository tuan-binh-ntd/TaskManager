using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");

            //Many to Many Relationship (AppUser, AppUserRole, AppRole)
            builder
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            //One to Many Relationship (AppUser, Project)
            builder
                .HasMany(u => u.Projects)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.LeaderId)
                .IsRequired();
        }
    }
}
