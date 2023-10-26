﻿using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IPriorityRepository : IRepository<Priority>
    {
        Task<IReadOnlyCollection<Priority>> GetByProjectId(Guid projectId);
        Task<Priority> GetById(Guid id);
        Priority Add(Priority priority);
        void AddRange(IReadOnlyCollection<Priority> priorities);
        void Update(Priority priority);
        void Delete(Guid id);
    }
}
