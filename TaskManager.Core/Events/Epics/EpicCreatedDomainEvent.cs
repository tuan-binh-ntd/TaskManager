namespace TaskManager.Core.Events.Epics
{
    public sealed class EpicCreatedDomainEvent(
        CreateEpicDto createEpicDto,
        Issue epic,
        IssueDetail issueDetail
        )
        : IDomainEvent
    {
        public CreateEpicDto CreateEpicDto { get; private set; } = createEpicDto;
        public Issue Epic { get; private set; } = epic;
        public IssueDetail IssueDetail { get; private set; } = issueDetail;
    }
}
