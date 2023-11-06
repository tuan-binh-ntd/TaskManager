namespace TaskManager.Core.ViewModel
{
    public class GetIssuesByEpicIdViewModel
    {
        public BacklogViewModel? Backlog { get; set; }
        public IReadOnlyCollection<SprintViewModel>? Sprints { get; set; }
        public EpicViewModel? Epics { get; set; }
    }
}
