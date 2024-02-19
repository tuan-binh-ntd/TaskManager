namespace TaskManager.Application.Labels.Commands.Update;

internal sealed class UpdateLabelCommandHandler(
    ILabelRepository labelRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateLabelCommand, Result<LabelViewModel>>
{
    private readonly ILabelRepository _labelRepository = labelRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LabelViewModel>> Handle(UpdateLabelCommand updateLabelCommand, CancellationToken cancellationToken)
    {
        var label = await _labelRepository.GetByIdAsync(updateLabelCommand.LabelId);
        if (label is null) return Result.Failure<LabelViewModel>(Error.NotFound);
        label.Name = updateLabelCommand.UpdateLabelDto.Name;
        label.Color = updateLabelCommand.UpdateLabelDto.Color;
        label.Description = updateLabelCommand.UpdateLabelDto.Description;

        _labelRepository.Update(label);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(label.Adapt<LabelViewModel>());
    }
}
