using MapsterMapper;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;
using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Infrastructure.Services
{
    public class VersionService : IVersionService
    {
        private readonly IVersionRepository _versionRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IMapper _mapper;

        public VersionService(
            IMapper mapper,
            IVersionRepository versionRepository,
            IStatusRepository statusRepository,
            IIssueRepository issueRepository)
        {
            _mapper = mapper;
            _versionRepository = versionRepository;
            _statusRepository = statusRepository;
            _issueRepository = issueRepository;
        }

        #region PrivateMethod
        //private async Task<IReadOnlyCollection<VersionViewModel>> ToVersionViewModels(IReadOnlyCollection<Version> versions)
        //{
        //    var versionViewModels = new List<VersionViewModel>();
        //    if (versions.Any())
        //    {
        //        foreach (var version in versions)
        //        {
        //            var versionViewModel = await ToVersionViewModel(version);
        //            versionViewModels.Add(versionViewModel);
        //        }
        //    }
        //    return versionViewModels;
        //}
        private Task<VersionViewModel> ToVersionViewModel(Version version)
        {
            var versionViewModel = _mapper.Map<VersionViewModel>(version);
            _versionRepository.LoadEntitiesRelationship(version);
            versionViewModel.Issues = version.Issues is not null ? _mapper.Map<IReadOnlyCollection<IssueViewModel>>(version.Issues) : null;
            return Task.FromResult(versionViewModel);
        }
        #endregion

        public async Task<VersionViewModel> AddIssues(AddIssuesToVersionDto addIssuesToVersionDto)
        {
            if (addIssuesToVersionDto.IssueIds is not null && addIssuesToVersionDto.IssueIds.Any())
            {
                var issues = await _issueRepository.GetByIds(addIssuesToVersionDto.IssueIds.ToList());
                if (issues.Any())
                {
                    foreach (var issue in issues)
                    {
                        issue.VersionId = addIssuesToVersionDto.VersionId;
                    }
                }
                _issueRepository.UpdateRange(issues);
                await _issueRepository.UnitOfWork.SaveChangesAsync();
                var version = await _versionRepository.GetById(addIssuesToVersionDto.VersionId);
                return await ToVersionViewModel(version);
            }
            else
            {
                return new VersionViewModel();
            }
        }
        public async Task<VersionViewModel> Create(CreateVersionDto createVersionDto)
        {
            var unreleasedStatus = await _statusRepository.GetUnreleasedStatus(createVersionDto.ProjectId);
            var version = _mapper.Map<Version>(createVersionDto);
            version.StatusId = unreleasedStatus.Id;
            _versionRepository.Add(version);
            await _versionRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<VersionViewModel>(version);
        }

        public async Task<Guid> Delete(Guid id)
        {
            _versionRepository.Delete(id);
            await _versionRepository.UnitOfWork.SaveChangesAsync();
            return id;
        }

        public async Task<IReadOnlyCollection<VersionViewModel>> GetByProjectId(Guid projectId)
        {
            var versions = await _versionRepository.GetByProjectId(projectId);
            return _mapper.Map<IReadOnlyCollection<VersionViewModel>>(versions);
        }

        public async Task<VersionViewModel> Update(Guid id, UpdateVersionDto updateVersionDto)
        {
            var version = await _versionRepository.GetById(id);
            if (version is null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(version));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            version = _mapper.Map<Version>(updateVersionDto);
            _versionRepository.Update(version);
            await _versionRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<VersionViewModel>(version);
        }
    }
}
