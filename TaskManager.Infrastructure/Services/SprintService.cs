using Mapster;
using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectConfigurationRepository _projectConfigurationRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ITransitionRepository _transitionRepository;
        private readonly IMapper _mapper;

        public SprintService(
            ISprintRepository sprintRepository,
            IProjectConfigurationRepository projectConfigurationRepository,
            IIssueRepository issueRepository,
            IStatusRepository statusRepository,
            ITransitionRepository transitionRepository,
            IMapper mapper
            )
        {
            _sprintRepository = sprintRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
            _issueRepository = issueRepository;
            _statusRepository = statusRepository;
            _transitionRepository = transitionRepository;
            _mapper = mapper;
        }

        public async Task<SprintViewModel> CreateNoFieldSprint(Guid projectId)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(projectId);
            int sprintIndex = projectConfiguration.SprintCode + 1;

            Sprint sprint = new()
            {
                Name = $"{projectConfiguration.Code} Sprint {sprintIndex}",
                StartDate = null,
                EndDate = null,
                Goal = string.Empty,
                ProjectId = projectId,
            };
            _sprintRepository.Add(sprint);
            var result = await _sprintRepository.UnitOfWork.SaveChangesAsync();

            if (result > 0)
            {
                projectConfiguration.SprintCode = sprintIndex;
                _projectConfigurationRepository.Update(projectConfiguration);
                await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();
            }
            return sprint.Adapt<SprintViewModel>();
        }

        public async Task<SprintViewModel> CreateSprint(CreateSprintDto createSprintDto)
        {
            var sprint = createSprintDto.Adapt<Sprint>();
            var sprintVM = _sprintRepository.Add(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprintVM;
        }

        public async Task<Guid> DeleteSprint(Guid id)
        {
            var issues = await _sprintRepository.GetIssues(id);
            _issueRepository.DeleteRange(issues);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            _sprintRepository.Delete(id);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<SprintViewModel> StartSprint(Guid projectId, Guid sprintId, UpdateSprintDto updateSprintDto)
        {
            var sprint = _sprintRepository.Get(sprintId);
            sprint = updateSprintDto.Adapt(sprint);
            _sprintRepository.Update(sprint!);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();

            var issues = await _sprintRepository.GetIssues(sprintId);
            var createTransition = _transitionRepository.GetCreateTransitionByProjectId(projectId);
            foreach (var issue in issues)
            {
                issue.StatusId = createTransition.ToStatusId;
            }
            _issueRepository.UpdateRange(issues);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var sprintViewModel = _mapper.Map<SprintViewModel>(sprint!);
            sprintViewModel.Issues = _mapper.Map<ICollection<IssueViewModel>>(issues);
            return sprintViewModel;
        }

        public async Task<SprintViewModel> UpdateSprint(Guid id, UpdateSprintDto updateSprintDto)
        {
            var sprint = _sprintRepository.Get(id);
            if (sprint is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(sprint));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            sprint = updateSprintDto.Adapt(sprint);
            _sprintRepository.Update(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprint.Adapt<SprintViewModel>();
        }
    }
}
