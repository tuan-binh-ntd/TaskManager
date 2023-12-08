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
    public class EpicsController : BaseController
    {
        private readonly IEpicService _epicService;

        public EpicsController(
            IEpicService epicService
            )
        {
            _epicService = epicService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(EpicViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(CreateEpicDto createEpicDto)
        {
            var res = await _epicService.CreateEpic(createEpicDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EpicViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, UpdateEpicDto updateEpicDto)
        {
            var res = await _epicService.UpdateEpic(id, updateEpicDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpPut("{id}/issues:add")]
        [ProducesResponseType(typeof(EpicViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddIssue(Guid id, AddIssueToEpicDto addIssueToEpicDto)
        {
            var res = await _epicService.AddIssueToEpic(issueId: addIssueToEpicDto.IssueId, epicId: id);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _epicService.DeleteEpic(id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
