using TaskManager.Application.StatusCategories.Queries.GetAll;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatusCategoriesController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<StatusCategoryViewModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Gets()
        => await Maybe<GetAllStatusCategoriesQuery>
        .From(new GetAllStatusCategoriesQuery())
        .Binding(query => Mediator.Send(query))
        .Match(Ok, NotFound);
}
