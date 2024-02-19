namespace TaskManager.Core.DTOs;

public class CreateStatusDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    //Relationship
    public Guid? ProjectId { get; set; }
    public Guid StatusCategoryId { get; set; }
    public bool AllowAnyStatus { get; set; }
}

public class UpdateStatusDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    //Relationship
    public Guid? StatusCategoryId { get; set; }
    public bool AllowAnyStatus { get; set; }
}
