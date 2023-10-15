using Mapster;
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

        public SprintService(
            ISprintRepository sprintRepository,
            IProjectConfigurationRepository projectConfigurationRepository
            )
        {
            _sprintRepository = sprintRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
        }

        public async Task<SprintViewModel> CreateNoFieldSprint(Guid projectId)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(projectId);
            int sprintIndex = projectConfiguration.SprintCode++;

            Sprint sprint = new()
            {
                Name = $"{projectConfiguration.Code} {sprintIndex}",
                StartDate = null,
                EndDate = null,
                Goal = string.Empty,
                ProjectId = projectId,
            };
            _sprintRepository.Add(sprint);
            var result = await _sprintRepository.UnitOfWork.SaveChangesAsync();

            if(result > 0)
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
