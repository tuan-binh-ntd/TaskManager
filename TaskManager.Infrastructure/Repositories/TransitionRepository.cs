using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
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
    }
}
