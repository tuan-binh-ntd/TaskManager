namespace TaskManager.Core.ViewModel;

public class WorkflowViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<TransitionViewModel> Transitions { get; set; } = new List<TransitionViewModel>();
}

public class TransitionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ProjectId { get; set; }
    public Guid? FromStatusId { get; set; }
    public Guid ToStatusId { get; set; }
}