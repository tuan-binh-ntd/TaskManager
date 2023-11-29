namespace TaskManager.Core.Exceptions
{
    public class LabelNullException : Exception
    {
        private const string _message = "The an instance of Label is null!";
        public LabelNullException()
            : base(_message) { }
    }
}
