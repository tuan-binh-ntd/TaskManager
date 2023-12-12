namespace TaskManager.Core.ViewModel;

public class TeamViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public IReadOnlyCollection<MemberViewModel>? Members { get; set; }
}

public class MemberViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
