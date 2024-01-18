namespace TaskManager.API.Controllers;

[Route("api/users/{userId}/[controller]")]
[ApiController]
public class TeamsController : BaseController
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TeamViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets(Guid userId)
    {
        var res = await _teamService.GetByUserId(userId);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeamViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var res = await _teamService.GetById(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TeamViewModel), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create(CreateTeamDto createTeamDto)
    {
        var res = await _teamService.Create(createTeamDto);
        return CustomResult(res, HttpStatusCode.Created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TeamViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, UpdateTeamDto updateTeamDto)
    {
        var res = await _teamService.Update(id, updateTeamDto);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var res = await _teamService.Delete(id);
        return CustomResult(res, HttpStatusCode.OK);
    }

    [HttpPut("members:add")]
    [ProducesResponseType(typeof(TeamViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddMember(AddMemberToTeamDto addMemberToTeamDto)
    {
        var res = await _teamService.AddMember(addMemberToTeamDto);
        return CustomResult(res, HttpStatusCode.OK);
    }
}
