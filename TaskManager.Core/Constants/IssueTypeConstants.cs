namespace TaskManager.Core.Constants;

public static class IssueTypeConstants
{
    public const string EpicName = "Epic";
    public const string StoryName = "Story";
    public const string BugName = "Bug";
    public const string TaskName = "Task";
    public const string SubtaskName = "Subtask";

    public const string EpicIcon = "epic";
    public const string StoryIcon = "story";
    public const string BugIcon = "bug";
    public const string TaskIcon = "task";
    public const string SubtaskIcon = "subtask";

    public const string EpicDesc = "Epics track collections of related bugs, stories, and tasks.";
    public const string StoryDesc = "Stories track functionality or features expressed as user goals.";
    public const string BugDesc = "Bugs track problems or errors.";
    public const string TaskDesc = "Tasks track small, distinct pieces of work.";
    public const string SubtaskDesc = "Subtasks track small pieces of work that are part of a larger task.";

    public const int OneLevel = 1;
    public const int TwoLevel = 2;
    public const int ThreeLevel = 3;
}
