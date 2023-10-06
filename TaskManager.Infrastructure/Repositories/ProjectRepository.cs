using Microsoft.EntityFrameworkCore;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Project Add(Project project)
        {
            return _context.Projects.Add(project).Entity;
        }

        public void Update(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);
            _context.Projects.Remove(project!);
        }

        public async Task<IReadOnlyCollection<Project>> GetAll()
        {
            var projects = await _context.Projects
                .Include(p => p.UserProjects!)
                .ThenInclude(up => up.User)
                .ToListAsync();
            return projects.AsReadOnly();
        }

        public async Task<Project> GetById(Guid id)
        {
            var project = await _context.Projects
                .Include(p => p.UserProjects!)
                .ThenInclude(up => up.User)
                .SingleOrDefaultAsync(p => p.Id == id);
            return project!;
        }

        public async Task<PaginationResult<Project>> GetByUserId(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput)
        {

            var query = from u in _context.Users
                        join up in _context.UserProjects.Where(up => up.UserId == userId) on u.Id equals up.UserId
                        join p in _context.Projects
                        .WhereIf(!string.IsNullOrWhiteSpace(filter.name), p => p.Name.Contains(filter.name))
                        .WhereIf(!string.IsNullOrWhiteSpace(filter.code), p => p.Code.Contains(filter.code))
                        on up.ProjectId equals p.Id
                        select p;

            return await query.Pagination(paginationInput);
        }

        public async Task<IReadOnlyCollection<Project>> GetByUserId(Guid userId, GetProjectByFilterDto filter)
        {
            var query = from u in _context.Users
                        join up in _context.UserProjects.Where(up => up.UserId == userId) on u.Id equals up.UserId
                        join p in _context.Projects
                        .WhereIf(!string.IsNullOrWhiteSpace(filter.name), p => p.Name.Contains(filter.name))
                        .WhereIf(!string.IsNullOrWhiteSpace(filter.code), p => p.Code.Contains(filter.code))
                        on up.ProjectId equals p.Id
                        select p;

            var projects = await query.ToListAsync();
            return projects.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<UserViewModel>> GetMembers(Guid projectId)
        {
            var members = await (from up in _context.UserProjects.Where(up => up.ProjectId == projectId)
                                 join u in _context.Users on up.UserId equals u.Id
                                 select new UserViewModel
                                 {
                                     Id = u.Id,
                                     Name = u.Name,
                                     Department = u.Department,
                                     Organization = u.Organization,
                                     AvatarUrl = u.AvatarUrl,
                                     JobTitle = u.JobTitle,
                                     Location = u.Location,
                                     Email = u.Email,
                                     Role = up.Role
                                 }).ToListAsync();
            return members.AsReadOnly();
        }

        public async Task<Project?> GetByCode(string code)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Code == code);
            return project;
        }
    }
}
