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

        public async Task<IReadOnlyCollection<Project>> GetAll()
        {
            var projects = await _context.Projects.ToListAsync();
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

        public async Task<object> GetByUserId(Guid userId, GetProjectByFilterDto input = null!)
        {
            var query = _context.Projects
                .Include(e => e.UserProjects!.Where(e => e.UserId == userId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.name), p => p.Name.Contains(input.name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.code), p => p.Code.Contains(input.code));

            if (input.pagenum is not default(int) || input.pagesize is not default(int))
            {
                int totalCount = await query.CountAsync();

                query = query.PageBy(input);

                PaginationResult<Project> data = new()
                {
                    TotalCount = totalCount,
                    TotalPage = Ceiling(input.pagesize, totalCount),
                    Content = await query.ToListAsync()
                };

                return data;

            }
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

        private static int Ceiling(int pageSize, int totalCount)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }

        public async Task<Project?> GetByCode(string code)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Code == code);
            return project;
        }
    }
}
