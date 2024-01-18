namespace TaskManager.Infrastructure.Repositories;

public class TransitionRepository : ITransitionRepository
{
    private readonly AppDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public TransitionRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Transition Add(Transition transition)
    {
        return _context.Transitions.Add(transition).Entity;
    }

    public void Delete(Guid id)
    {
        var transittion = _context.Transitions.FirstOrDefault(e => e.Id == id);
        _context.Transitions.Remove(transittion!);
    }

    public void Update(Transition transition)
    {
        _context.Entry(transition).State = EntityState.Modified;
    }

    public void AddRange(ICollection<Transition> transitions)
    {
        _context.Transitions.AddRange(transitions);
    }

    public Transition GetCreateTransitionByProjectId(Guid projectId)
    {
        var transition = _context.Transitions.AsNoTracking().Where(t => t.ProjectId == projectId && t.Name == CoreConstants.CreateTransitionName).FirstOrDefault();
        return transition!;
    }

    public async Task<Transition?> GetById(Guid id)
    {
        var transition = await _context.Transitions.Where(t => t.Id == id).FirstOrDefaultAsync();
        return transition;
    }

    public async Task<IReadOnlyCollection<TransitionViewModel>> GetByProjectId(Guid projectId)
    {
        var transitionViewModels = await _context.Transitions
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .Select(t => new TransitionViewModel
            {
                Id = t.Id,
                Name = t.Name,
                FromStatusId = t.FromStatusId,
                ToStatusId = t.ToStatusId,
            }).ToListAsync();

        return transitionViewModels.AsReadOnly();
    }
}
