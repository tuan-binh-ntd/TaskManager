using TaskManager.Application.UserProjects.Commands.Create;

namespace TaskManager.API.Controllers;

[Route("api/projects/{projectId:guid}/[controller]")]
[ApiController]
public class MembersController(IMediator mediator)
    : ApiController(mediator)
{

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<MemberProjectViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetMembers(Guid projectId, [FromQuery] PaginationInput paginationInput)
        => await Maybe<GetMembersOfProjectQuery>
        .From(new GetMembersOfProjectQuery(projectId, paginationInput))
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);

    [HttpPost]
    [ProducesResponseType(typeof(ProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddMember([FromBody] AddMemberToProjectDto addMemberToProjectDto)
        => await Result.Success(new CreateUserProjectCommand(addMemberToProjectDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MemberProjectViewModel), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberProjectDto updateMemberProjectDto)
        => await Result.Success(new UpdateUserProjectCommand(id, updateMemberProjectDto))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(Guid projectId, Guid id)
        => await Result.Success(new DeleteUserProjectCommand(projectId, id))
        .Bind(command => Mediator.Send(command))
        .Match(Ok, BadRequest);
}
