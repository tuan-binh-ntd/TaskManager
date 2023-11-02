﻿using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IIssueHistoryRepository _issueHistoryRepository;
        private readonly IIssueDetailRepository _issueDetailRepository;
        private readonly IProjectConfigurationRepository _projectConfigurationRepository;
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly ITransitionRepository _transitionRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public IssueService(
            IIssueRepository issueRepository,
            IIssueHistoryRepository issueHistoryRepository,
            IIssueDetailRepository issueDetailRepository,
            IProjectConfigurationRepository projectConfigurationRepository,
            IIssueTypeRepository issueTypeRepository,
            ITransitionRepository transitionRepository,
            ICommentRepository commentRepository,
            UserManager<AppUser> userManager,
            IMapper mapper
            )
        {
            _issueRepository = issueRepository;
            _issueHistoryRepository = issueHistoryRepository;
            _issueDetailRepository = issueDetailRepository;
            _projectConfigurationRepository = projectConfigurationRepository;
            _issueTypeRepository = issueTypeRepository;
            _transitionRepository = transitionRepository;
            _commentRepository = commentRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        #region Private Method
        private IReadOnlyCollection<IssueViewModel> ToIssueViewModels(IReadOnlyCollection<Issue> issues)
        {
            var issueViewModels = new List<IssueViewModel>();
            if (issues.Any())
            {
                foreach (var issue in issues)
                {
                    var issueViewModel = ToIssueViewModel(issue);
                    issueViewModels.Add(issueViewModel);
                }
            }
            return issueViewModels.AsReadOnly();
        }

        private IssueViewModel ToIssueViewModel(Issue issue)
        {
            _issueRepository.LoadEntitiesRelationship(issue);
            var issueViewModel = _mapper.Map<IssueViewModel>(issue);

            if (issue.IssueDetail is not null)
            {
                var issueDetail = _mapper.Map<IssueDetailViewModel>(issue.IssueDetail);
                issueViewModel.IssueDetail = issueDetail;
            }
            if (issue.IssueHistories is not null && issue.IssueHistories.Any())
            {
                var issueHistories = _mapper.Map<ICollection<IssueHistoryViewModel>>(issue.IssueHistories);
                issueViewModel.IssueHistories = issueHistories;
            }
            if (issue.Comments is not null && issue.Comments.Any())
            {
                var comments = _mapper.Map<ICollection<CommentViewModel>>(issue.Comments);
                issueViewModel.Comments = comments;
            }
            if (issue.Attachments is not null && issue.Attachments.Any())
            {
                var attachments = _mapper.Map<ICollection<AttachmentViewModel>>(issue.Attachments);
                issueViewModel.Attachments = attachments;
            }
            if (issue.IssueType is not null)
            {
                var issueType = _mapper.Map<IssueTypeViewModel>(issue.IssueType);
                issueViewModel.IssueType = issueType;
            }
            if (issue.Status is not null)
            {
                var status = _mapper.Map<StatusViewModel>(issue.Status);
                issueViewModel.Status = status;
            }
            return issueViewModel;
        }
        #endregion

        public async Task<IssueViewModel> CreateIssue(CreateIssueDto createIssueDto, Guid? sprintId = null, Guid? backlogId = null)
        {
            var issue = createIssueDto.Adapt<Issue>();

            if (sprintId is not null)
            {
                issue.SprintId = sprintId;
            }
            else
            {
                issue.BacklogId = backlogId;
            }

            _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueDetail = new IssueDetail()
            {
                ReporterId = createIssueDto.CreatorUserId,
                StoryPointEstimate = 0,
                Label = string.Empty,
                IssueId = issue.Id,
            };

            _issueDetailRepository.Add(issueDetail);
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

            var issueHis = new IssueHistory
            {
                Name = "created the Issue",
                CreatorUserId = createIssueDto.CreatorUserId,
                IssueId = issue.Id,
            };

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();
            return ToIssueViewModel(issue);
        }

        public async Task<IssueViewModel> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto)
        {
            var issue = await _issueRepository.Get(id);
            if (issue is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(issue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            if (updateIssueDto.UserIds is not null && updateIssueDto.UserIds.Any())
            {
                foreach (var item in updateIssueDto.UserIds)
                {
                    var user = await _userManager.FindByIdAsync(item.ToString());
                    if (user is not null && issue.Watcher is not null && issue.Watcher.Users is not null)
                    {
                        var watcher = new User()
                        {
                            Name = user.Name,
                            Email = user.Email!
                        };
                        issue.Watcher.Users.Add(watcher);
                    }
                }
            }
            issue = updateIssueDto.Adapt(issue);
            _issueRepository.Update(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueDetail = await _issueDetailRepository.GetById(id);
            if (updateIssueDto.StoryPointEstimate is not null)
            {
                issueDetail.StoryPointEstimate = (int)updateIssueDto.StoryPointEstimate;
            }
            if (updateIssueDto.ReporterId is not null)
            {
                issueDetail.ReporterId = (Guid)updateIssueDto.ReporterId;
            }
            if (updateIssueDto.AssigneeId is not null)
            {
                issueDetail.AssigneeId = updateIssueDto.AssigneeId;
            }
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();
            return ToIssueViewModel(issue);
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetBySprintId(Guid sprintId)
        {
            var issues = await _issueRepository.GetIssueBySprintId(sprintId);
            return ToIssueViewModels(issues);
        }

        public async Task<IReadOnlyCollection<IssueViewModel>> GetByBacklogId(Guid backlogId)
        {
            var issues = await _issueRepository.GetIssueByBacklogId(backlogId);
            return ToIssueViewModels(issues);
        }

        public async Task<IssueViewModel> CreateIssueByName(CreateIssueByNameDto createIssueByNameDto, Guid? sprintId = null, Guid? backlogId = null)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createIssueByNameDto.ProjectId);
            int issueIndex = projectConfiguration.IssueCode + 1;
            var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createIssueByNameDto.ProjectId);
            var creatorUser = await _userManager.FindByIdAsync(createIssueByNameDto.CreatorUserId.ToString());

            var issue = new Issue()
            {
                Name = createIssueByNameDto.Name,
                IssueTypeId = createIssueByNameDto.IssueTypeId,
                Code = $"{projectConfiguration.Code}-{issueIndex}",
                StatusId = createTransition.ToStatusId,
                PriorityId = projectConfiguration.DefaultPriorityId,
                Watcher = new()
            };

            if (sprintId is not null)
            {
                issue.SprintId = sprintId;
            }
            else
            {
                issue.BacklogId = backlogId;
            }

            if (creatorUser is not null && issue.Watcher is not null && issue.Watcher.Users is not null)
            {
                var user = new User()
                {
                    Name = creatorUser.Name,
                    Email = creatorUser.Email!
                };
                issue.Watcher.Users.Add(user);
            }

            _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueDetail = new IssueDetail()
            {
                ReporterId = createIssueByNameDto.CreatorUserId,
                StoryPointEstimate = 0,
                Label = string.Empty,
                IssueId = issue.Id,
                AssigneeId = projectConfiguration.DefaultAssigneeId,
            };

            _issueDetailRepository.Add(issueDetail);
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

            var issueHis = new IssueHistory
            {
                Name = "created the Issue",
                CreatorUserId = createIssueByNameDto.CreatorUserId,
                IssueId = issue.Id,
            };

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

            projectConfiguration.IssueCode = issueIndex;
            _projectConfigurationRepository.Update(projectConfiguration);
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

            return ToIssueViewModel(issue);
        }

        public async Task<Guid> DeleteIssue(Guid id)
        {
            _issueRepository.Delete(id);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<ChildIssueViewModel> CreateChildIssue(CreateChildIssueDto createChildIssueDto)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createChildIssueDto.ProjectId);
            int issueIndex = projectConfiguration.IssueCode + 1;
            var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createChildIssueDto.ProjectId);
            var creatorUser = await _userManager.FindByIdAsync(createChildIssueDto.CreatorUserId.ToString());

            var issueType = await _issueTypeRepository.GetSubtask();

            var issue = new Issue()
            {
                Name = createChildIssueDto.Name,
                IssueTypeId = issueType.Id,
                Code = $"{projectConfiguration.Code}-{issueIndex}",
                ParentId = createChildIssueDto.ParentId,
                PriorityId = projectConfiguration.DefaultPriorityId,
                StatusId = createTransition.ToStatusId,
                Watcher = new()
            };

            if (creatorUser is not null && issue.Watcher is not null && issue.Watcher.Users is not null)
            {
                var user = new User()
                {
                    Name = creatorUser.Name,
                    Email = creatorUser.Email!
                };
                issue.Watcher.Users.Add(user);
            }

            _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            var issueVM = _mapper.Map<ChildIssueViewModel>(issue);

            var issueDetail = new IssueDetail()
            {
                ReporterId = createChildIssueDto.CreatorUserId,
                StoryPointEstimate = 0,
                Label = string.Empty,
                IssueId = issue.Id,
                AssigneeId = projectConfiguration.DefaultAssigneeId,
            };

            _issueDetailRepository.Add(issueDetail);
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

            var issueHis = new IssueHistory
            {
                Name = "created the Issue",
                CreatorUserId = createChildIssueDto.CreatorUserId,
                IssueId = issue.Id,
            };

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

            projectConfiguration.IssueCode = issueIndex;
            _projectConfigurationRepository.Update(projectConfiguration);
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

            return issueVM;
        }

        public async Task<IssueViewModel> AddEpic(Guid issueId, Guid epicId)
        {
            var issue = await _issueRepository.Get(issueId);
            if (issue is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(issue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            issue.ParentId = epicId;
            _issueRepository.Update(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            return ToIssueViewModel(issue);
        }

        public async Task<EpicViewModel> CreateEpic(CreateEpicDto createEpicDto)
        {
            var projectConfiguration = _projectConfigurationRepository.GetByProjectId(createEpicDto.ProjectId);
            int issueIndex = projectConfiguration.IssueCode + 1;
            var createTransition = _transitionRepository.GetCreateTransitionByProjectId(createEpicDto.ProjectId);
            var creatorUser = await _userManager.FindByIdAsync(createEpicDto.CreatorUserId.ToString());

            var issueType = await _issueTypeRepository.GetEpic();

            var issue = new Issue()
            {
                Name = createEpicDto.Name,
                IssueTypeId = issueType.Id,
                Code = $"{projectConfiguration.Code}-{issueIndex}",
                ProjectId = createEpicDto.ProjectId,
                StatusId = createTransition.ToStatusId,
                Watcher = new()
            };

            if (creatorUser is not null && issue.Watcher is not null && issue.Watcher.Users is not null)
            {
                var user = new User()
                {
                    Name = creatorUser.Name,
                    Email = creatorUser.Email!
                };
                issue.Watcher.Users.Add(user);
            }

            _issueRepository.Add(issue);
            await _issueRepository.UnitOfWork.SaveChangesAsync();
            var epicViewModel = _mapper.Map<EpicViewModel>(issue);

            var issueDetail = new IssueDetail()
            {
                ReporterId = createEpicDto.CreatorUserId,
                StoryPointEstimate = 0,
                Label = string.Empty,
                IssueId = issue.Id,
            };

            _issueDetailRepository.Add(issueDetail);
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();

            var issueHis = new IssueHistory
            {
                Name = "created the Issue",
                CreatorUserId = createEpicDto.CreatorUserId,
                IssueId = issue.Id,
            };

            _issueHistoryRepository.Add(issueHis);
            await _issueHistoryRepository.UnitOfWork.SaveChangesAsync();

            projectConfiguration.IssueCode = issueIndex;
            _projectConfigurationRepository.Update(projectConfiguration);
            await _projectConfigurationRepository.UnitOfWork.SaveChangesAsync();

            return epicViewModel;
        }

        public async Task<IssueViewModel> GetById(Guid id)
        {
            var issue = await _issueRepository.Get(id);
            return ToIssueViewModel(issue!);
        }

        public async Task<IReadOnlyCollection<IssueHistoryViewModel>> GetHistoriesByIssueId(Guid issueId)
        {
            var issueHistories = await _issueHistoryRepository.GetByIssueId(issueId);
            return _mapper.Map<IReadOnlyCollection<IssueHistoryViewModel>>(issueHistories);
        }

        public async Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueId(Guid issueId)
        {
            var comments = await _commentRepository.GetByIssueId(issueId);
            return _mapper.Map<IReadOnlyCollection<CommentViewModel>>(comments);
        }

        public async Task<IssueViewModel> UpdateEpic(Guid id, UpdateEpicDto updateEpicDto)
        {
            var epic = await _issueRepository.Get(id);
            if (epic is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(epic));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            epic = updateEpicDto.Adapt(epic);
            _issueRepository.Update(epic);
            await _issueRepository.UnitOfWork.SaveChangesAsync();

            var issueDetail = await _issueDetailRepository.GetById(id);
            if (updateEpicDto.StoryPointEstimate is not null)
            {
                issueDetail.StoryPointEstimate = (int)updateEpicDto.StoryPointEstimate;
            }
            if (updateEpicDto.ReporterId is not null)
            {
                issueDetail.ReporterId = (Guid)updateEpicDto.ReporterId;
            }
            if (updateEpicDto.AssigneeId is not null)
            {
                issueDetail.AssigneeId = updateEpicDto.AssigneeId;
            }
            await _issueDetailRepository.UnitOfWork.SaveChangesAsync();
            return ToIssueViewModel(epic);
        }
    }
}
