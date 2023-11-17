using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public AttachmentRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public AttachmentViewModel Add(Attachment attachment)
        {
            return _context.Attachments.Add(attachment).Entity.Adapt<AttachmentViewModel>();
        }

        public async Task<IReadOnlyCollection<AttachmentViewModel>> Gets()
        {
            var attachments = await _context.Attachments.AsNoTracking().ProjectToType<AttachmentViewModel>().ToListAsync();
            return attachments.AsReadOnly();
        }

        public void Update(Attachment attachment)
        {
            _context.Entry(attachment).State = EntityState.Modified;
        }

        public void Delete(Attachment attachment)
        {
            _context.Attachments.Remove(attachment);
        }

        public async Task<Attachment?> GetById(Guid id)
        {
            var attachment = await _context.Attachments.Where(a => a.Id == id).FirstOrDefaultAsync();
            return attachment;
        }
    }
}
