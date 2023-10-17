using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/[controller]")]
    [ApiController]
    public class EpicsController : BaseController
    {
        private readonly IIssueService _issueService;

        public EpicsController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEpicDto createEpicDto)
        {
            var res = await _issueService.CreateEpic(createEpicDto);
            return CustomResult(res, HttpStatusCode.Created);
        }
    }
}
