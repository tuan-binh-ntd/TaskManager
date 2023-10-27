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
        // Priority
        public const string LowestName = "Lowest";
        public const string LowName = "Low";
        public const string MediumName = "Medium";
        public const string HighName = "High";
        public const string HighestName = "Highest";
    }
}
