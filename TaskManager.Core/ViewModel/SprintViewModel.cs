namespace TaskManager.Core.ViewModel
{
    public class SprintViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Goal { get; set; } = string.Empty;
        public ICollection<IssueViewModel>? Issues { get; set; }
    }
}
