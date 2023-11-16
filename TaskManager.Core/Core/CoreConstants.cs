namespace TaskManager.Core.Core
{
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
        public const string IssueCreatedName = "Issue Created (System)";
        public const string IssueUpdatedName = "Issue Updated (System)";
        public const string IssueAssignedName = "Issue Assigned (System)";
        public const string IssueResolvedName = "Issue Resolved (System)";
        public const string IssueClosedName = "Issue Closed (System)";
        public const string IssueCommentedName = "Issue Commented (System)";
        public const string IssueCommentEditedName = "Issue Comment Edited (System)";
        public const string IssueCommentDeletedName = "Issue Comment Deleted (System)";
        public const string IssueReopenedName = "Issue Reopened (System)";
        public const string IssueDeletedName = "Issue Deleted (System)";
        public const string IssueMovedName = "Issue Moved (System)";
        public const string WorkLoggedOnIssueName = "Work Logged On Issue (System)";
        public const string WorkStartedOnIssueName = "Work Started On Issue (System)";
        public const string WorkStoppedOnIssueName = "Work Stopped On Issue (System)";
        public const string IssueWorklogUpdatedName = "Issue Worklog Updated (System)";
        public const string IssueWorklogDeletedName = "Issue Worklog Deleted (System)";
        public const string GenericEventName = "Generic Event (System)";
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
        public const string ProductOwnerName = "Product Owner";
        public const string ScrumMasterName = "Scrum Master";
        public const string DeveloperName = "Developer";
        // Permission
        public const string SprintPermissionName = "Sprint";

        public const string CreateSprintPermissionName = "Create Sprint";
        public const string UpdateSprintPermissionName = "Update Sprint";
        public const string DeleteSprintPermissionName = "Delete Sprint";

        public const string EpicPermissionName = "Epic";

        public const string CreateEpicPermissionName = "Create Epic";
        public const string UpdateEpicPermissionName = "Update Epic";
        public const string DeleteEpicPermissionName = "Delete Epic";

        public const string IssuePermissionName = "Issue";

        public const string CreateIssuePermissionName = "Create Issue";
        public const string UpdateIssuePermissionName = "Update Issue";
        public const string DeleteIssuePermissionName = "Delete Issue";

        public const string ChildIssuePermissionName = "Child Issue";

        public const string CreateChildIssuePermission = "Create Child Issue";
        public const string UpdateChildIssuePermission = "Update Child Issue";
        public const string DeleteChildIssuePermission = "Delete Child Issue";

        public const string CommentPermissionName = "Comment";

        public const string CreateCommentPermissionName = "Create Comment";
        public const string UpdateCommentPermissionName = "Update Comment";
        public const string DeleteCommentPermissionName = "Delete Comment";

        public const string IssueTypePermissionName = "Issue Type";

        public const string CreateIssueTypePermissionName = "Create Issue Type";
        public const string UpdateIssueTypePermissionName = "Update Issue Type";
        public const string DeleteIssueTypePermissionName = "Delete Issue Type";

        public const string StatusPermissionName = "Status";

        public const string CreateStatusPermissionName = "Create Status";
        public const string UpdateStatusPermissionName = "Update Status";
        public const string DeleteStatusPermissionName = "Delete Status";

        public const string PriorityPermissionName = "Priority";

        public const string CreatePriorityPermissionName = "Create Priority";
        public const string UpdatePriorityPermissionName = "Update Priority";
        public const string DeletePriorityPermissionName = "Delete Priority";
        // Complete Sprint
        public const string NewSprintOption = "New sprint";
        public const string BacklogOption = "Backlog";
    }
}
