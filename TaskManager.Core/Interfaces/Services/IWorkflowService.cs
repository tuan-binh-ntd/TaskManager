﻿using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services;

public interface IWorkflowService
{
    Task<WorkflowViewModel> GetWorkflowViewModelByProjectId(Guid projectId);
    Task UpdateWorkflow(Guid id, UpdateWorkflowDto updateWorkflowDto);
}