namespace TaskManager.Core.DTOs;

public class BuidEmailTemplateBaseDto
{
    public string SenderName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string IssueCode { get; set; } = string.Empty;
    public string IssueName { get; set; } = string.Empty;
    public string EmailContent { get; set; } = string.Empty;
}

public abstract class IssueEmailContentBase
{
    public string ReporterName { get; set; } = string.Empty;
    public DateTime IssueCreationTime { get; set; }
}

public class CreatedIssueEmailContentDto : IssueEmailContentBase
{
    public string IssueTypeName { get; set; } = string.Empty;
    public string AssigneeName { get; set; } = string.Empty;
    public string PriorityName { get; set; } = string.Empty;
}

public class ChangeStatusIssueEmailContentDto : IssueEmailContentBase
{
    public string FromStatusName { get; set; } = string.Empty;
    public string ToStatusName { get; set; } = string.Empty;
}

public class AddNewCommentIssueEmailContentDto : IssueEmailContentBase
{
    public string CommentContent { get; set; } = string.Empty;
}

public class ChangeSprintIssueEmailContentDto : IssueEmailContentBase
{
    public string FromSprintName { get; set; } = string.Empty;
    public string ToSprintName { get; set; } = string.Empty;
}
public class ChangeNameIssueEmailContentDto : IssueEmailContentBase
{
    public string FromName { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;
}

public class ChangeDueDateIssueEmailContentDto : IssueEmailContentBase
{
    public DateTime DueDate { get; set; }
}

public class ChangeStartDateIssueEmailContentDto : IssueEmailContentBase
{
    public DateTime StartDate { get; set; }
}

public class ChangeAssigneeIssueEmailContentDto : IssueEmailContentBase
{
    public string FromAssigneeName { get; set; } = string.Empty;
    public string ToAssigneeName { get; set; } = string.Empty;
}

public class ChangeReporterIssueEmailContentDto : IssueEmailContentBase
{
    public string FromReporterName { get; set; } = string.Empty;
    public string ToReporterName { get; set; } = string.Empty;
}

public class ChangeParentIssueEmailContentDto : IssueEmailContentBase
{
    public string FromParentName { get; set; } = string.Empty;
    public string ToParentName { get; set; } = string.Empty;
}

public class ChangeIssueTypeIssueEmailContentDto : IssueEmailContentBase
{
    public string FromIssueTypeName { get; set; } = string.Empty;
    public string ToIssueTypeName { get; set; } = string.Empty;
}

public class ChangePriorityIssueEmailContentDto : IssueEmailContentBase
{
    public string FromPriorityName { get; set; } = string.Empty;
    public string ToPriorityName { get; set; } = string.Empty;
}

public class ChangeSPEIssueEmailContentDto : IssueEmailContentBase
{
    public string FromSPEName { get; set; } = string.Empty;
    public string ToSPEName { get; set; } = string.Empty;
}

public class AddNewAttachmentIssueEmailContentDto : IssueEmailContentBase
{
    public string AttachmentName { get; set; } = string.Empty;
}

public class DeleteNewAttachmentIssueEmailContentDto : IssueEmailContentBase
{
    public string AttachmentName { get; set; } = string.Empty;
}