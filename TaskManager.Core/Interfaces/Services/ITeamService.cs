using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    public interface ITeamService
    {
        Task<TeamViewModel> GetById(Guid id);
        Task<TeamViewModel> Create(CreateTeamDto createTeamDto);
        Task<TeamViewModel> Update(Guid id, UpdateTeamDto updateTeamDto);
        Task<Guid> Delete(Guid id);
        Task<IReadOnlyCollection<TeamViewModel>> GetByUserId(Guid userId);
        Task<TeamViewModel> AddMember(AddMemberToTeamDto addMemberToTeamDto);
    }
}
