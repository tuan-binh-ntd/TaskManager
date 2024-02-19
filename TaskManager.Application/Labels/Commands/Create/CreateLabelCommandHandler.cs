namespace TaskManager.Application.Labels.Commands.Create;

internal sealed class CreateLabelCommandHandler(
    ILabelRepository labelRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateLabelCommand, Result<LabelViewModel>>
{
    private readonly ILabelRepository _labelRepository = labelRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LabelViewModel>> Handle(CreateLabelCommand createLabelCommand, CancellationToken cancellationToken)
    {
        var label = new Label
        {
            ProjectId = createLabelCommand.ProjectId,
            Name = createLabelCommand.CreateLabelDto.Name,
            Description = createLabelCommand.CreateLabelDto.Description,
            Color = createLabelCommand.CreateLabelDto.Color,
        };

        _labelRepository.Insert(label);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(label.Adapt<LabelViewModel>());
    }
}
