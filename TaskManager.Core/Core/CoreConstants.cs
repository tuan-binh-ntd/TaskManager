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
        // Status
        public const string AnyStatusName = "ANY STATUS";
        public const string StartStatusName = "START";
        public const string TodoStatusName = "TO DO";
        public const string InProgresstatusName = "IN PROGRESS";
        public const string DoneStatusName = "DONE";
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
        public const string NormalName = "Normal";
        public const string MediumName = "Medium";
        public const string HighName = "High";
        public const string UrgentName = "Urgent";

    }
}
