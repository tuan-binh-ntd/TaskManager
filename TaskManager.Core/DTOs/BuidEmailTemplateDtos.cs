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
    protected IssueEmailContentBase(string reporterName, DateTime issueCreationTime)
    {
        ReporterName = reporterName;
        IssueCreationTime = issueCreationTime;
    }

    public string ReporterName { get; set; } = string.Empty;
    public DateTime IssueCreationTime { get; set; }
}

public class CreatedIssueEmailContentDto : IssueEmailContentBase
{
    public CreatedIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string IssueTypeName { get; set; } = string.Empty;
    public string AssigneeName { get; set; } = string.Empty;
    public string PriorityName { get; set; } = string.Empty;
}

public class ChangeStatusIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeStatusIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromStatusName { get; set; } = string.Empty;
    public string ToStatusName { get; set; } = string.Empty;
}

public class AddNewCommentIssueEmailContentDto : IssueEmailContentBase
{
    public AddNewCommentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string content) : base(reporterName, issueCreationTime)
    {
        CommentContent = content;
    }

    public string CommentContent { get; set; } = string.Empty;
}

public class ChangeSprintIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeSprintIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromSprintName { get; set; } = string.Empty;
    public string ToSprintName { get; set; } = string.Empty;
}
public class ChangeNameIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeNameIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromName { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;
}

public class ChangeDueDateIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeDueDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string fromDueDate, string toDueDate) : base(reporterName, issueCreationTime)
    {
        FromDueDate = fromDueDate;
        ToDueDate = toDueDate;
    }

    public ChangeDueDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
        FromDueDate = string.Empty;
        ToDueDate = string.Empty;
    }

    public string FromDueDate { get; set; }
    public string ToDueDate { get; set; }
}

public class ChangeStartDateIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeStartDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string fromStartDate, string toStartDate) : base(reporterName, issueCreationTime)
    {
        FromStartDate = fromStartDate;
        ToStartDate = toStartDate;
    }

    public ChangeStartDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
        FromStartDate = string.Empty;
        ToStartDate = string.Empty;
    }

    public string FromStartDate { get; set; }
    public string ToStartDate { get; set; }
}

public class ChangeAssigneeIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeAssigneeIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromAssigneeName { get; set; } = string.Empty;
    public string ToAssigneeName { get; set; } = string.Empty;
}

public class ChangeReporterIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeReporterIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromReporterName { get; set; } = string.Empty;
    public string ToReporterName { get; set; } = string.Empty;
}

public class ChangeParentIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeParentIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromParentName { get; set; } = string.Empty;
    public string ToParentName { get; set; } = string.Empty;
}

public class ChangeIssueTypeIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeIssueTypeIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromIssueTypeName { get; set; } = string.Empty;
    public string ToIssueTypeName { get; set; } = string.Empty;
}

public class ChangePriorityIssueEmailContentDto : IssueEmailContentBase
{
    public ChangePriorityIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromPriorityName { get; set; } = string.Empty;
    public string ToPriorityName { get; set; } = string.Empty;
}

public class ChangeSPEIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeSPEIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string FromSPEName { get; set; } = string.Empty;
    public string ToSPEName { get; set; } = string.Empty;
}

public class AddNewAttachmentIssueEmailContentDto : IssueEmailContentBase
{
    public AddNewAttachmentIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string AttachmentName { get; set; } = string.Empty;
}

public class DeleteNewAttachmentIssueEmailContentDto : IssueEmailContentBase
{
    public DeleteNewAttachmentIssueEmailContentDto(string reporterName, DateTime issueCreationTime) : base(reporterName, issueCreationTime)
    {
    }

    public string AttachmentName { get; set; } = string.Empty;
}

public class DeleteCommentIssueEmailContentDto : IssueEmailContentBase
{
    public DeleteCommentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string content) : base(reporterName, issueCreationTime)
    {
        CommentContent = content;
    }

    public string CommentContent { get; set; } = string.Empty;
}