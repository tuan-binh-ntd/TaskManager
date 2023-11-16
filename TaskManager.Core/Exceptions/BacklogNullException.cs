namespace TaskManager.Core.Exceptions
{
    public class BacklogNullException : Exception
    {
        private const string _message = "The an instance of Backlog is null!";
        public BacklogNullException()
            : base(_message) { }
    }
}
