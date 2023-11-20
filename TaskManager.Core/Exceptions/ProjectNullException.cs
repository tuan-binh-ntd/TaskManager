namespace TaskManager.Core.Exceptions
{
    public class ProjectNullException : Exception
    {
        private const string _message = "The an instance of Project is null!";
        public ProjectNullException()
            : base(_message) { }
    }
}
