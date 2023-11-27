namespace TaskManager.Core.DTOs
{
    public class GetSprintByFilterDto
    {
        public IReadOnlyCollection<Guid> EpicIds { get; set; } = new List<Guid>();
        //#pragma warning disable IDE1006 // Naming Styles
        //        public Guid? labelid { get; set; }
        //#pragma warning restore IDE1006 // Naming Styles
        public Guid? IssueTypeId { get; set; }
        public Guid? SprintId { get; set; }
        public string? SearchKey { get; set; } = string.Empty;
    }
}
