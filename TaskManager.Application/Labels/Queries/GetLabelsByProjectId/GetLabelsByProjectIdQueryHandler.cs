namespace TaskManager.Application.Labels.Queries.GetLabelsByProjectId;

internal sealed class GetLabelsByProjectIdQueryHandler(
    ILabelRepository labelRepository
    )
    : IQueryHandler<GetLabelsByProjectIdQuery, Maybe<object>>
{
    private readonly ILabelRepository _labelRepository = labelRepository;

    public async Task<Maybe<object>> Handle(GetLabelsByProjectIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PaginationInput.IsPaging())
        {
            var paginationResult = await _labelRepository.GetLabelViewModelsByProjectIdPagingAsync(request.ProjectId, request.PaginationInput);
            return Maybe<PaginationResult<LabelViewModel>>.From(paginationResult);
        }
        var labels = await _labelRepository.GetLabelsByProjectIdAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<LabelViewModel>>.From(labels.Adapt<IReadOnlyCollection<LabelViewModel>>());
    }
}
