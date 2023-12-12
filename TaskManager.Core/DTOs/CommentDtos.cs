namespace TaskManager.Core.DTOs;

public class CreateCommentDto
{
    public Guid CreatorUserId { get; set; }
    public bool IsEdited { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class UpdateCommentDto
{
    public string Content { get; set; } = string.Empty;
}
