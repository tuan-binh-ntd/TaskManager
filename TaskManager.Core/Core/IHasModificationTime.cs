namespace TaskManager.Core.Core
{
    public interface IHasModificationTime
    {
        DateTime? ModificationTime { get; set; }
    }
}
