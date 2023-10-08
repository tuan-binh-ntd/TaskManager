namespace TaskManager.Core.ViewModel
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsFavourite { get; set; } = false;
        public UserViewModel? Leader { get; set; }
        public ICollection<UserViewModel>? Members { get; set; }
        public BacklogViewModel? Backlog{ get; set; }
    }

    public class BacklogViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
