namespace TaskManager.Core.ViewModel
{
    public class PermissionGroupViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IReadOnlyCollection<PermissionViewModel>? Permissions { get; set; }
    }

    public class PermissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public bool ViewPermission { get; set; }
        public bool EditPermission { get; set; }
    }
}
