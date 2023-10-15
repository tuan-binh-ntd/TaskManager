using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class SprintsController : BaseController
    {
        private readonly ISprintService _sprintService;

        public SprintsController(
            ISprintService sprintService
            )
        {
            _sprintService = sprintService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSprintDto createSprintDto)
        {
            var res = await _sprintService.CreateSprint(createSprintDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateSprintDto updateSprintDto)
        {
            var res = await _sprintService.UpdateSprint(id, updateSprintDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost("{:no-field}")]
        public async Task<IActionResult> Create(Guid projectId)
        {
            var res = await _sprintService.CreateNoFieldSprint(projectId);
            return CustomResult(res, HttpStatusCode.Created);
        }
    }
}
