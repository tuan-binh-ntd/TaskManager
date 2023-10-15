using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IProjectConfigurationRepository : IRepository<ProjectConfiguration>
    {
        ProjectConfiguration Add(ProjectConfiguration projectConfiguration);
        void Update(ProjectConfiguration projectConfiguration);
        ProjectConfiguration GetByProjectId(Guid projectId);
    }
}
