namespace TaskManager.Core.Exceptions
{
    public class PermissionGroupNullException : Exception
    {
        private const string _message = "The an instance of PermissionGroup is null!";
        public PermissionGroupNullException()
            : base(_message) { }
    }
}
