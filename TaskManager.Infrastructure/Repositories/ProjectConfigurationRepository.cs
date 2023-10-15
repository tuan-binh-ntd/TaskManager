using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectConfigurationRepository : IProjectConfigurationRepository
    {
        private readonly AppDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProjectConfigurationRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ProjectConfiguration Add(ProjectConfiguration projectConfiguration)
        {
            return _context.ProjectConfigurations.Add(projectConfiguration).Entity;
        }

        public ProjectConfiguration GetByProjectId(Guid projectId)
        {
            return _context.ProjectConfigurations.Where(e => e.ProjectId == projectId).FirstOrDefault()!;
        }

        public void Update(ProjectConfiguration projectConfiguration)
        {
            _context.Entry(projectConfiguration).State = EntityState.Modified;
        }
    }
}
