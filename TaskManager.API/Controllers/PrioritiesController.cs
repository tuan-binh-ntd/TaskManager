using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class PrioritiesController : BaseController
    {
        private readonly IPriorityService _priorityService;

        public PrioritiesController(IPriorityService priorityService)
        {
            _priorityService = priorityService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid projectId, [FromQuery] PaginationInput paginationInput)
        {
            var res = await _priorityService.GetByProjectId(projectId, paginationInput);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PriorityViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(CreatePriorityDto createPriorityDto)
        {
            var res = await _priorityService.Create(createPriorityDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PriorityViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, UpdatePriorityDto updatePriorityDto)
        {
            var res = await _priorityService.Update(id, updatePriorityDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid newId)
        {
            var res = await _priorityService.Delete(id, newId);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
