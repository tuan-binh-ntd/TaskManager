﻿namespace TaskManager.Persistence.Data.Config;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        //Many to Many Relationship (AppUser, UserTeam, Team)
        builder
            .HasMany(t => t.UserTeams)
            .WithOne(ut => ut.Team)
            .HasForeignKey(ut => ut.TeamId)
            .IsRequired();
    }
}
