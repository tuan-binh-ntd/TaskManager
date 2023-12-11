﻿using Mapster;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class LabelService : ILabelService
{
    private readonly ILabelRepository _labelRepository;

    public LabelService(ILabelRepository labelRepository)
    {
        _labelRepository = labelRepository;
    }

    public async Task<LabelViewModel> Create(Guid projectId, CreateLabelDto createLabelDto)
    {
        var label = new Label
        {
            ProjectId = projectId,
            Name = createLabelDto.Name
        };

        _labelRepository.Add(label);
        await _labelRepository.UnitOfWork.SaveChangesAsync();
        return label.Adapt<LabelViewModel>();
    }

    public async Task<Guid> Delete(Guid id)
    {
        var label = await _labelRepository.GetById(id) ?? throw new LabelNullException();
        _labelRepository.Delete(label);
        await _labelRepository.UnitOfWork.SaveChangesAsync();
        return id;
    }

    public async Task<IReadOnlyCollection<LabelViewModel>> GetLabelsByProjectId(Guid projectId)
    {
        var labels = await _labelRepository.GetByProjectId(projectId);
        return labels.Adapt<IReadOnlyCollection<LabelViewModel>>();
    }

    public async Task<LabelViewModel> Update(Guid id, UpdateLabelDto updateLabelDto)
    {
        var label = await _labelRepository.GetById(id) ?? throw new LabelNullException();
        label.Name = updateLabelDto.Name;
        _labelRepository.Update(label);
        await _labelRepository.UnitOfWork.SaveChangesAsync();
        return label.Adapt<LabelViewModel>();
    }
}