using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusCategoriesController : BaseController
    {
        private readonly IStatusService _statusService;

        public StatusCategoriesController(
            IStatusService statusService
            )
        {
            _statusService = statusService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<StatusCategoryViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Gets()
        {
            var res = await _statusService.GetStatusCategoryViewModels();
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
