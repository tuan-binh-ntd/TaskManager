namespace TaskManager.API.Controllers;

[Authorize]
public class ApiController : BaseController
{
    protected ApiController(IMediator mediator) => Mediator = mediator;

    protected IMediator Mediator { get; }

    protected IActionResult BadRequest(Error error) => CustomResult(error, StatusCodes.Status400BadRequest);

    protected new IActionResult Ok(object value) => CustomResult(value);

    protected IActionResult Ok(Guid value) => CustomResult(value);

    protected new IActionResult NotFound() => base.NotFound();
}
