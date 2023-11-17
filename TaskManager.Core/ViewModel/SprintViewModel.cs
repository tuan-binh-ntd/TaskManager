namespace TaskManager.Core.ViewModel
{
    public class SprintViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Goal { get; set; } = string.Empty;
        public bool IsStart { get; set; } = false;
        public bool IsComplete { get; set; } = false;
        public IReadOnlyCollection<IssueViewModel>? Issues { get; set; }
    }
}
