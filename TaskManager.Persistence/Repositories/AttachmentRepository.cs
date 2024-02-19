namespace TaskManager.Persistence.Repositories;

public class AttachmentRepository(IDbContext context) : GenericRepository<Attachment>(context)
    , IAttachmentRepository
{
    public async Task<IReadOnlyCollection<AttachmentViewModel>> GetByIssueId(Guid issueId)
    {
        var attachments = await Entity
            .AsNoTracking()
            .Where(a => a.IssueId == issueId)
            .ProjectToType<AttachmentViewModel>()
            .ToListAsync();

        return attachments.AsReadOnly();
    }
}
