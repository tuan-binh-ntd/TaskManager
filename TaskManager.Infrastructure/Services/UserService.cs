using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJWTTokenService _jwtTokenService;
    private readonly IUserProjectRepository _userProjectRepository;
    private readonly ICriteriaRepository _criteriaRepository;
    private readonly IFilterRepository _filterRepository;
    private readonly IStatusCategoryRepository _statusCategoryRepository;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJWTTokenService jwtTokenService,
        IUserProjectRepository userProjectRepository,
        ICriteriaRepository criteriaRepository,
        IFilterRepository filterRepository,
        IStatusCategoryRepository statusCategoryRepository,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _userProjectRepository = userProjectRepository;
        _criteriaRepository = criteriaRepository;
        _filterRepository = filterRepository;
        _statusCategoryRepository = statusCategoryRepository;
        _mapper = mapper;
    }

    #region PrivateMethod
    private async Task CreateDefaultFilters(Guid userId)
    {
        var criterias = await _criteriaRepository.Gets();

        var assigneeCriteria = criterias.Where(c => c.Name == CoreConstants.AssigneeCriteriaName).FirstOrDefault();
        var resolutionCriteria = criterias.Where(c => c.Name == CoreConstants.ResolutionCriteriaName).FirstOrDefault();
        var reporterCriteria = criterias.Where(c => c.Name == CoreConstants.ReporterCriteriaName).FirstOrDefault();
        var statusCategoryCriteria = criterias.Where(c => c.Name == CoreConstants.StatusCategoryCriteriaName).FirstOrDefault();
        var createdCriteria = criterias.Where(c => c.Name == CoreConstants.CreatedCriteriaName).FirstOrDefault();
        var resolvedCriteria = criterias.Where(c => c.Name == CoreConstants.ResolvedCriteriaName).FirstOrDefault();
        var updatedCriteria = criterias.Where(c => c.Name == CoreConstants.UpdatedCriteriaName).FirstOrDefault();
        var doneStatusCategory = await _statusCategoryRepository.GetDone() ?? throw new StatusCategoryNullException();

        var filterConfiguration = new FilterConfiguration()
        {
            Assginee = new AssigneeCriteria()
            {
                CurrentUserId = userId,
            },
        };

        #region Create My open issues filter
        var myOpenIssues = new Filter()
        {
            Name = CoreConstants.MyOpenIssuesFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        #region Create Reported by me filter
        filterConfiguration = new FilterConfiguration()
        {
            Reporter = new ReporterCriteria()
            {
                CurrentUserId = userId,
            },
        };

        var reportedByMe = new Filter()
        {
            Name = CoreConstants.ReportedByMeFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        #region Create All issues filter
        var allIssues = new Filter()
        {
            Name = CoreConstants.AllIssuesFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = null!,
            CreatorUserId = userId,
        };
        #endregion

        #region Create open issues
        filterConfiguration = new FilterConfiguration()
        {
            Resolution = new ResolutionCriteria()
            {
                Unresolved = true,
                Done = false,
            },
        };

        var openIssues = new Filter()
        {
            Name = CoreConstants.OpenIssuesFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        #region Create done issues
        filterConfiguration = new FilterConfiguration()
        {
            StatusCategory = new StatusCategoryCriteria()
            {
                StatusCategoryIds = new List<Guid>()
                {
                    doneStatusCategory.Id
                }
            },
        };
        var doneIssues = new Filter()
        {
            Name = CoreConstants.DoneIssuesFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        #region Create Created recently filter
        filterConfiguration = new FilterConfiguration()
        {
            Created = new CreatedCriteria()
            {
                MoreThan = new MoreThan(1, CoreConstants.WeekUnit)
            },
        };

        var createdRecently = new Filter()
        {
            Name = CoreConstants.CreatedRecentlyFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        #region Create resolved recently filter
        filterConfiguration = new FilterConfiguration()
        {
            Resolved = new ResolvedCriteria()
            {
                MoreThan = new MoreThan(1, CoreConstants.WeekUnit)
            },
        };

        var resolvedRecently = new Filter()
        {
            Name = CoreConstants.ResolvedRecentlyFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        #region Create updated recently filter
        filterConfiguration = new FilterConfiguration()
        {
            Updated = new UpdatedCriteria()
            {
                MoreThan = new MoreThan(1, CoreConstants.WeekUnit)
            },
        };

        var updatedRecently = new Filter()
        {
            Name = CoreConstants.UpdatedRecentlyFilterName,
            Type = CoreConstants.DefaultFiltersType,
            Configuration = filterConfiguration.ToJson(),
            CreatorUserId = userId,
        };
        #endregion

        var filters = new List<Filter>
        {
            myOpenIssues, reportedByMe, allIssues, openIssues, doneIssues, createdRecently, resolvedRecently, updatedRecently
        };

        _filterRepository.AddRange(filters);
        await _filterRepository.UnitOfWork.SaveChangesAsync();
    }
    #endregion

    public async Task<object> SignIn(LoginDto loginDto)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(e => e.Email == loginDto.Email);

        if (user is null) return "Invalid email";

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return "Incorrect name or password";

        UserViewModel res = _mapper.Map<UserViewModel>(user);

        var permissionGroups = await _userProjectRepository.GetByUserId(user.Id);
        res.PermissionGroups = permissionGroups;

        res.Token = await _jwtTokenService.CreateToken(user);

        return res;
    }

    public async Task<object> SignUp(SignUpDto signUpDto)
    {
        if (await CheckEmailExists(signUpDto.Email)) return "Email is taken";

        var user = _mapper.Map<AppUser>(signUpDto);
        user.UserName = signUpDto.Email;
        user.Name = signUpDto.Name;

        var result = await _userManager.CreateAsync(user, signUpDto.Password);

        if (!result.Succeeded) return result.Errors;

        await CreateDefaultFilters(user.Id);

        UserViewModel res = new()
        {
            Token = await _jwtTokenService.CreateToken(user),
            Id = user.Id,
            Email = user.Email,
        };

        return res;
    }

    public async Task<bool> CheckEmailExists(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<bool> CheckUsernameExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username);
    }

    public async Task<object?> ChangePassword(string id, PasswordDto passwordDto)
    {
        AppUser? user = await _userManager.FindByIdAsync(id);
        if (user is null) return null;

        var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

        if (!result.Succeeded) return result.Errors;

        UserViewModel res = _mapper.Map<UserViewModel>(user);

        return res;
    }

    public async Task<IReadOnlyCollection<UserViewModel>> Gets(GetUserByFilterDto filter)
    {
        var users = await _userManager.Users.ToListAsync();
        if (filter.Name != null)
        {
            users = users.Where(users => users.Name.ToLower().Trim().
            Contains(filter.Name.ToLower().Trim())).ToList();
        }
        return users.Adapt<IReadOnlyCollection<UserViewModel>>();
    }

    public async Task<UserViewModel?> GetById(Guid id)
    {
        AppUser? user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return null;
        return user.Adapt<UserViewModel>();
    }

    public async Task<UserViewModel> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.FindByIdAsync(id.ToString()) ?? throw new UserNullException();
        user.Name = !string.IsNullOrWhiteSpace(updateUserDto.Name) ? updateUserDto.Name : user.Name;
        user.Department = !string.IsNullOrWhiteSpace(updateUserDto.Department) ? updateUserDto.Department : user.Department;
        user.Organization = !string.IsNullOrWhiteSpace(updateUserDto.Organization) ? updateUserDto.Organization : user.Organization;
        user.AvatarUrl = !string.IsNullOrWhiteSpace(updateUserDto.AvatarUrl) ? updateUserDto.AvatarUrl : user.AvatarUrl;
        user.JobTitle = !string.IsNullOrWhiteSpace(updateUserDto.JobTitle) ? updateUserDto.JobTitle : user.JobTitle;
        user.Location = !string.IsNullOrWhiteSpace(updateUserDto.Location) ? updateUserDto.Location : user.Location;
        await _userManager.UpdateAsync(user);
        return user.Adapt<UserViewModel>();
    }
}
