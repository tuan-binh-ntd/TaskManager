namespace TaskManager.Core.ViewModel
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid LeaderId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public bool IsFavourite { get; set; } = false;
    }
}
