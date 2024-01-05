namespace TaskManager.Core.Core;

public static class CoreConstants
{
    // LINQ extension
    public const string Ascending = "asc";
    public const string Descending = "desc";
    // Role
    public const string LeaderRole = "Leader";
    // Status Category
    public const string ToDoCode = "To-do";
    public const string InProgressCode = "In-Progress";
    public const string DoneCode = "Done";
    public const string HideCode = "Hide";
    public const string VersionCode = "Version";
    // Status
    public const string AnyStatusName = "ANY STATUS";
    public const string StartStatusName = "START";
    public const string TodoStatusName = "TO DO";
    public const string InProgresstatusName = "IN PROGRESS";
    public const string DoneStatusName = "DONE";
    public const string UnreleasedStatusName = "UNRELEASED";
    public const string ReleasedStatusName = "RELEASED";
    public const string ArchivedStatusName = "ARCHIVED";
    // Transition
    public const string CreateTransitionName = "Create";
    public const string WorkingTransitionName = "Working";
    public const string FinishedTransitionName = "Finished";
    // IssueType
    public const string EpicName = "Epic";
    public const string StoryName = "Story";
    public const string BugName = "Bug";
    public const string TaskName = "Task";
    public const string SubTaskName = "Subtask";

    public const string EpicIcon = "epic";
    public const string StoryIcon = "story";
    public const string BugIcon = "bug";
    public const string TaskIcon = "task";
    public const string SubTaskIcon = "subtask";
    // Priority
    public const string LowestName = "Lowest";
    public const string LowName = "Low";
    public const string MediumName = "Medium";
    public const string HighName = "High";
    public const string HighestName = "Highest";

    public const string LowestIcon = "fa-solid fa-angles-down";
    public const string LowIcon = "fa-solid fa-arrow-down";
    public const string MediumIcon = "fa-solid fa-equals";
    public const string HighIcon = "fa-solid fa-chevron-up";
    public const string HighestIcon = "fa-solid fa-angles-up";

    public const string LowestColor = "#55a557";
    public const string LowColor = "#2a8735";
    public const string MediumColor = "#ffab00";
    public const string HighColor = "#ff8d71";
    public const string HighestColor = "#fe6d4b";
    // Issue Event
    public const string IssueCreatedName = "An issue is created";
    public const string IssueEditedName = "An issue is edited";
    public const string SomeoneAssignedToAIssueName = "You're assigned to an issue";
    public const string IssueDeletedName = "An issue is deleted";
    public const string IssueMovedName = "An issue is moved";
    public const string SomeoneMadeACommentName = "Someone made a comment";
    public const string CommentEditedName = "A comment is edited";
    public const string CommentDeletedName = "A comment is deleted";
    public const string SomeoneMadeAAttachmentName = "Someone added a attachment";
    public const string AttachmentDeletedName = "A attachment is deleted";
    // User Filter
    public const string EditorType = "Editor";
    public const string ViewerType = "Viewer";
    public const string NoneType = "None";
    // Criteria
    public const string ProjectCriteriaName = "Project";
    public const string TypeCriteriaName = "Type";
    public const string StatusCriteriaName = "Status";
    public const string AssigneeCriteriaName = "Assignee";
    public const string CreatedCriteriaName = "Created";
    public const string DueDateCriteriaName = "Due date";
    public const string FixVersionsCriteriaName = "Fix versions";
    public const string LabelsCriteriaName = "Labels";
    public const string PriorityCriteriaName = "Priority";
    public const string ReporterCriteriaName = "Reporter";
    public const string ResolutionCriteriaName = "Resolution";
    public const string ResolvedCriteriaName = "Resolved";
    public const string SprintCriteriaName = "Sprint";
    public const string StatusCategoryCriteriaName = "Status Category";
    public const string SummaryCriteriaName = "Summary";
    public const string UpdatedCriteriaName = "Updated";
    // Filter
    public const string DefaultFiltersType = "DEFAULT FILTERS";
    public const string StaredFiltersType = "STARED FILTERS";
    public const string CreatedUserFiltersType = "CREATED USER FILTERS";

    public const string MyOpenIssuesFilterName = "My open issues";
    public const string ReportedByMeFilterName = "Reported by me";
    public const string AllIssuesFilterName = "All issues";
    public const string OpenIssuesFilterName = "Open issues";
    public const string DoneIssuesFilterName = "Done issues";
    public const string ViewedRecentlyFilterName = "Viewed recently";
    public const string CreatedRecentlyFilterName = "Created recently";
    public const string ResolvedRecentlyFilterName = "Resolved recently";
    public const string UpdatedRecentlyFilterName = "Updated recently";
    // Filter Criteria
    public const string MinutesUnit = "Minutes";
    public const string HoursUnit = "Hours";
    public const string DaysUnit = "Days";
    public const string WeekUnit = "Weeks";
    // Role
    public const string ProjectLeadName = "Project Lead";
    public const string ProductOwnerName = "Product Owner";
    public const string ScrumMasterName = "Scrum Master";
    public const string DeveloperName = "Developer";
    // Permission
    public const string TimelinePermissionName = "Timeline";
    public const string BacklogPermissionName = "Backlog";
    public const string BoardPermissionName = "Board";
    public const string CodePermissionName = "Code";
    public const string DetailsPermissionName = "Details";
    public const string IssueTypesPermissionName = "Issue Types";
    public const string PrioritiesPermissionName = "Priorities";
    public const string StatusesPermissionName = "Statuses";
    public const string NotificationsPermissionName = "Notifications";
    public const string AccessPermissionName = "Access";
    public const string FeaturesPermissionName = "Features";
    public const string ProjectPermissionName = "Project";
    // Complete Sprint
    public const string NewSprintOption = "New sprint";
    public const string BacklogOption = "Backlog";
    // Text to image service
    public const int BlockSize = 4194304;
    // Dashboard
    public const string AssigneeType = "Assignee";
    public const string ReporterType = "Reporter";
    public const string AllType = "all";
    // Avatar
    public const string AccessTokenAvatar = "?sv=2022-11-02&ss=bfqt&srt=o&sp=rwdlacupiytfx&se=2024-11-12T10:35:35Z&st=2023-12-11T02:35:35Z&spr=https,http&sig=wpS08Fr4qcC%2FjL2BqJtKhD7JgYTnc%2FvrpcRu2%2BC0mBM%3D";
    public const string AnonymousAvatar = "https://ntdhunter.file.core.windows.net/ntdhunter/AvatarOfUser/42-428449_anonymous-avatar-face-book-hd-png-download.png";
}
