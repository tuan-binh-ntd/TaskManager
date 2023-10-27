using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IVersionRepository : IRepository<Version>
    {
        Task<IReadOnlyCollection<Version>> GetByProjectId(Guid projectId);
        Task<Version> GetById(Guid id);
        Version Add(Version version);
        void Update(Version version);
        void Delete(Guid id);
        void LoadEntitiesRelationship(Version version);
    }
}
