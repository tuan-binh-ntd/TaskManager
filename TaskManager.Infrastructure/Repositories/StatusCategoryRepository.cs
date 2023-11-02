﻿using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class StatusCategoryRepository : IStatusCategoryRepository
    {
        private readonly AppDbContext _context;

        public StatusCategoryRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IReadOnlyCollection<StatusCategory> Gets()
        {
            var statusCategories = _context.StatusCategories.AsNoTracking().ToList();
            return statusCategories.AsReadOnly();
        }
    }
}
