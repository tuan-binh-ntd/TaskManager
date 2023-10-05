namespace TaskManager.Core.DTOs
{
    public class CreateSprintDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Goal { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }

    public class UpdateSprintDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Goal { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }
}
