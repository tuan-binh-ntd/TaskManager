using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

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
        [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(CreateSprintDto createSprintDto)
        {
            var res = await _sprintService.CreateSprint(createSprintDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, UpdateSprintDto updateSprintDto)
        {
            var res = await _sprintService.UpdateSprint(id, updateSprintDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost(":no-field")]
        [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(Guid projectId)
        {
            var res = await _sprintService.CreateNoFieldSprint(projectId);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _sprintService.DeleteSprint(id);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPut("{id}:start")]
        [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Start(Guid projectId, Guid id, UpdateSprintDto updateSprintDto)
        {
            var res = await _sprintService.StartSprint(projectId, sprintId: id, updateSprintDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPut("{id}:complete")]
        [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Complete(Guid id, Guid projectId, CompleteSprintDto completeSprintDto)
        {
            var res = await _sprintService.CompleteSprint(id, projectId, completeSprintDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SprintViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid projectId, Guid id)
        {
            var res = await _sprintService.GetById(projectId ,id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
