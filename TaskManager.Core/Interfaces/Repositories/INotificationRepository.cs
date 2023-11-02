using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<Notification> GetById(Guid id);
        Task<IReadOnlyCollection<Notification>> GetByUserId(Guid userId);
        Notification Add(Notification notification);
        void Update(Notification notification);
        void Delete(Guid id);
    }
}
