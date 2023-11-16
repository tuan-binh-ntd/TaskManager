﻿using Mapster;
using MapsterMapper;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
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
        private readonly ITransitionRepository _transitionRepository;
        private readonly IBacklogRepository _backlogRepository;
        private readonly IMapper _mapper;

        public SprintService(
            ISprintRepository sprintRepository,
            IProjectConfigurationRepository projectConfigurationRepository,
            IIssueRepository issueRepository,
            ITransitionRepository transitionRepository,
            IBacklogRepository backlogRepository,
            IMapper mapper
            )
        {
            _sprintRepository = sprintRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
            _issueRepository = issueRepository;
            _transitionRepository = transitionRepository;
            _backlogRepository = backlogRepository;
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
                IsStart = false,
                IsComplete = false,
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
            sprint!.IsStart = true;
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
            var sprint = _sprintRepository.Get(id) ?? throw new SprintNullException();
            sprint = updateSprintDto.Adapt(sprint);
            _sprintRepository.Update(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprint.Adapt<SprintViewModel>();
        }

        public async Task<SprintViewModel> CompleteSprint(Guid sprintId, Guid projectId, CompleteSprintDto completeSprintDto)
        {
            var sprint = _sprintRepository.Get(sprintId) ?? throw new SprintNullException();

            var issues = await _issueRepository.GetNotDoneIssuesBySprintId(sprintId, projectId);

            if (completeSprintDto.Option == CoreConstants.NewSprintOption)
            {
                var newSprint = await CreateNoFieldSprint(projectId);

                foreach (var issue in issues)
                {
                    issue.SprintId = newSprint.Id;
                }
                _issueRepository.UpdateRange(issues);
                await _issueRepository.UnitOfWork.SaveChangesAsync();
            }
            else if (completeSprintDto.Option == CoreConstants.BacklogOption)
            {
                var backlog = await _backlogRepository.GetByProjectId(projectId) ?? throw new BacklogNullException();
                foreach (var issue in issues)
                {
                    issue.SprintId = null;
                    issue.BacklogId = backlog.Id;
                }
                _issueRepository.UpdateRange(issues);
                await _issueRepository.UnitOfWork.SaveChangesAsync();
            }
            else
            {
                if (completeSprintDto.SprintId is Guid specificSprintId)
                {
                    var specificSprint = _sprintRepository.Get(specificSprintId) ?? throw new SprintNullException();
                    foreach (var issue in issues)
                    {
                        issue.SprintId = specificSprint.Id;
                    }
                    _issueRepository.UpdateRange(issues);
                    await _issueRepository.UnitOfWork.SaveChangesAsync();
                }
            }

            sprint.IsComplete = true;
            sprint.IsStart = false;
            _sprintRepository.Update(sprint);
            await _sprintRepository.UnitOfWork.SaveChangesAsync();
            return sprint.Adapt<SprintViewModel>();
        }
    }
}
