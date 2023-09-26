namespace TaskManager.Core.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public Guid LeaderId { get; set; }
        public Guid PacklogId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string AvartarUrl { get; set; } = string.Empty;
    }
}
