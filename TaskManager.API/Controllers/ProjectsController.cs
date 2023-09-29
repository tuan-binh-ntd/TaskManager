using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : BaseController
    {
        public ProjectsController(
            )
        {
        }

        //[HttpPost("{id}/user")]
        //public async Task<IActionResult> AddUser(Guid id, )
        //{


        //    return CustomResult(role, HttpStatusCode.OK);
        //}
    }
}
