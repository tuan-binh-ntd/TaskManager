﻿namespace TaskManager.Persistence.Data.Config;

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

        //Many to Many Relationship (AppUser, UserProject, Project)
        builder
            .HasMany(u => u.UserProjects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .IsRequired();

        //Many to Many Relationship (AppUser, UserTeam, Team)
        builder
            .HasMany(u => u.UserTeams)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .IsRequired();

        //One to Many Relationship (AppUser, UserNotification)
        builder
            .HasMany(u => u.Notifications)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .IsRequired();
    }
}
