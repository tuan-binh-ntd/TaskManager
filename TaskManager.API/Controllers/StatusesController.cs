﻿using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.API.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class StatusesController : BaseController
    {
        private readonly IStatusService _statusService;

        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(StatusViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(CreateStatusDto createStatusDto)
        {
            var res = await _statusService.Create(createStatusDto);
            return CustomResult(res, HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StatusViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, UpdateStatusDto updateStatusDto)
        {
            var res = await _statusService.Update(id, updateStatusDto);
            return CustomResult(res, HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _statusService.Delete(id);
            return CustomResult(res, HttpStatusCode.OK);
        }
    }
}
