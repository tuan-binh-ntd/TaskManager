using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        #region Private method
        private async Task<TeamViewModel> ToTeamViewModel(Team team)
        {
            var teamViewModel = _mapper.Map<TeamViewModel>(team);
            var members = await _teamRepository.GetMembers(team.Id);
            teamViewModel.Members = _mapper.Map<IReadOnlyCollection<MemberViewModel>>(members);
            return teamViewModel;
        }

        private async Task<IReadOnlyCollection<TeamViewModel>> ToTeamViewModels(IReadOnlyCollection<Team> teams)
        {
            var teamViewModels = new List<TeamViewModel>();
            if (teams.Any())
            {
                foreach (var team in teams)
                {
                    var teamViewModel = await ToTeamViewModel(team);
                    teamViewModels.Add(teamViewModel);
                }
            }
            return teamViewModels.AsReadOnly();
        }
        #endregion

        public async Task<TeamViewModel> Create(CreateTeamDto createTeamDto)
        {
            var team = _mapper.Map<Team>(createTeamDto);
            if (createTeamDto.UserIds is not null && createTeamDto.UserIds.Any())
            {
                team.UserTeams = new List<UserTeam>();
                foreach (var item in createTeamDto.UserIds)
                {

                    var userTeam = new UserTeam()
                    {
                        TeamId = team.Id,
                        UserId = item
                    };
                    team.UserTeams.Add(userTeam);
                }
            }
            _teamRepository.Add(team);
            await _teamRepository.UnitOfWork.SaveChangesAsync();
            return await ToTeamViewModel(team);
        }

        public async Task<Guid> Delete(Guid id)
        {
            _teamRepository.Delete(id);
            await _teamRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<TeamViewModel> GetById(Guid id)
        {
            var team = await _teamRepository.GetById(id);
            return await ToTeamViewModel(team);
        }

        public async Task<IReadOnlyCollection<TeamViewModel>> GetByUserId(Guid userId)
        {
            var teams = await _teamRepository.GetByUserId(userId);
            return await ToTeamViewModels(teams);
        }

        public async Task<TeamViewModel> Update(Guid id, UpdateTeamDto updateTeamDto)
        {
            var team = await _teamRepository.GetById(id);
            if (team is null)
            {
                throw new ArgumentNullException(nameof(team));
            }
            team = _mapper.Map<Team>(updateTeamDto);
            _teamRepository.Update(team);
            await _teamRepository.UnitOfWork.SaveChangesAsync();
            return await ToTeamViewModel(team);
        }

        public async Task<TeamViewModel> AddMember(AddMemberToTeamDto addMemberToTeamDto)
        {
            var team = await _teamRepository.GetById(addMemberToTeamDto.TeamId);
            if (team is null)
            {
                throw new ArgumentNullException(nameof(team));
            }
            _teamRepository.LoadEntitiesRelationship(team);

            if (team.UserTeams is not null && team.UserTeams.Any())
            {
                if (addMemberToTeamDto.UserIds.Any())
                {
                    foreach (var item in addMemberToTeamDto.UserIds)
                    {
                        var userTeam = new UserTeam()
                        {
                            TeamId = team.Id,
                            UserId = item
                        };
                    }
                }
            }

            return await ToTeamViewModel(team);
        }
    }
}
