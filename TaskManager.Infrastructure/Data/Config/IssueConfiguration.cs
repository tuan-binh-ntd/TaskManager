﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Data.Config
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder
                .HasMany(i => i.IssueHistories)
                .WithOne(ih => ih.Issue)
                .HasForeignKey(ih => ih.IssueId)
                .IsRequired();

            builder
                .HasMany(i => i.Comments)
                .WithOne(ih => ih.Issue)
                .HasForeignKey(ih => ih.IssueId)
                .IsRequired();

            builder
                .HasMany(i => i.Attachments)
                .WithOne(ih => ih.Issue)
                .HasForeignKey(ih => ih.IssueId)
                .IsRequired();

            builder
                .HasOne(i => i.IssueDetail)
                .WithOne(ih => ih.Issue)
                .IsRequired();
        }
    }
}
