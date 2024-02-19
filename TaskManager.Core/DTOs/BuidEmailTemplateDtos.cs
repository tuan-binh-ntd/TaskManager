namespace TaskManager.Core.DTOs;

public class BuidEmailTemplateBaseDto
{
    public BuidEmailTemplateBaseDto(string senderName, string actionName, string projectName, string issueCode, string issueName, string emailContent, string projectCode, Guid issueId)
    {
        SenderName = senderName;
        ActionName = actionName;
        ProjectName = projectName;
        IssueCode = issueCode;
        IssueName = issueName;
        EmailContent = emailContent;
        ProjectCode = projectCode;
        IssueId = issueId;
        IssueLink = $"{EmailConstants.FrontEndUrl}project/{projectCode}/backlog/{issueId}";
        ProjectLink = $"{EmailConstants.FrontEndUrl}project/{projectCode}/backlog";
    }

    public BuidEmailTemplateBaseDto(string actionName, string emailContent, Guid issueId)
    {
        ActionName = actionName;
        EmailContent = emailContent;
        IssueId = issueId;
    }

    public string SenderName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string IssueCode { get; set; } = string.Empty;
    public string IssueName { get; set; } = string.Empty;
    public string EmailContent { get; set; } = string.Empty;
    public string ProjectCode { get; set; } = string.Empty;
    public Guid IssueId { get; set; }
    public string IssueLink { get; set; } = string.Empty;
    public string ProjectLink { get; set; } = string.Empty;
}

public abstract class IssueEmailContentBase
{
    protected IssueEmailContentBase(string reporterName, DateTime issueCreationTime, string avatarUrl)
    {
        ReporterName = reporterName;
        IssueCreationTime = issueCreationTime;
        AvatarUrl = avatarUrl + TextToImageConstants.AccessTokenAvatar;
    }

    public string ReporterName { get; set; } = string.Empty;
    public DateTime IssueCreationTime { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
}

public class CreatedIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string IssueTypeName { get; set; } = string.Empty;
    public string AssigneeName { get; set; } = string.Empty;
    public string PriorityName { get; set; } = string.Empty;
    public string AssigneeAvatarUrl { get; set; } = string.Empty;
}

public class ChangeStatusIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromStatusName { get; set; } = string.Empty;
    public string ToStatusName { get; set; } = string.Empty;
}

public class AddNewCommentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string content, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string CommentContent { get; set; } = content;
}

public class ChangeSprintIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromSprintName { get; set; } = string.Empty;
    public string ToSprintName { get; set; } = string.Empty;
}
public class ChangeNameIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromName { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;
}

public class ChangeDueDateIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeDueDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string fromDueDate, string toDueDate, string avatarUrl) : base(reporterName, issueCreationTime, avatarUrl)
    {
        FromDueDate = fromDueDate;
        ToDueDate = toDueDate;
    }

    public ChangeDueDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : base(reporterName, issueCreationTime, avatarUrl)
    {
        FromDueDate = string.Empty;
        ToDueDate = string.Empty;
    }

    public string FromDueDate { get; set; }
    public string ToDueDate { get; set; }
}

public class ChangeStartDateIssueEmailContentDto : IssueEmailContentBase
{
    public ChangeStartDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string fromStartDate, string toStartDate, string avatarUrl) : base(reporterName, issueCreationTime, avatarUrl)
    {
        FromStartDate = fromStartDate;
        ToStartDate = toStartDate;
    }

    public ChangeStartDateIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : base(reporterName, issueCreationTime, avatarUrl)
    {
        FromStartDate = string.Empty;
        ToStartDate = string.Empty;
    }

    public string FromStartDate { get; set; }
    public string ToStartDate { get; set; }
}

public class ChangeAssigneeIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromAssigneeName { get; set; } = string.Empty;
    public string ToAssigneeName { get; set; } = string.Empty;
}

public class ChangeReporterIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromReporterName { get; set; } = string.Empty;
    public string ToReporterName { get; set; } = string.Empty;
}

public class ChangeParentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromParentName { get; set; } = string.Empty;
    public string ToParentName { get; set; } = string.Empty;
}

public class ChangeIssueTypeIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromIssueTypeName { get; set; } = string.Empty;
    public string ToIssueTypeName { get; set; } = string.Empty;
}

public class ChangePriorityIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromPriorityName { get; set; } = string.Empty;
    public string ToPriorityName { get; set; } = string.Empty;
}

public class ChangeSPEIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string FromSPEName { get; set; } = string.Empty;
    public string ToSPEName { get; set; } = string.Empty;
}

public class AddNewAttachmentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string AttachmentName { get; set; } = string.Empty;
}

public class DeleteNewAttachmentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string AttachmentName { get; set; } = string.Empty;
}

public class DeleteCommentIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string content, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string CommentContent { get; set; } = content;
}

public class DeletedIssueEmailContentDto(string reporterName, DateTime issueCreationTime, string avatarUrl) : IssueEmailContentBase(reporterName, issueCreationTime, avatarUrl)
{
    public string IssueName { get; set; } = string.Empty;
}