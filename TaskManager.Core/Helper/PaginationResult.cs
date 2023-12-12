namespace TaskManager.Core.Helper;;

public class PaginationResult<TList>
{
    public IReadOnlyCollection<TList>? Content { get; set; }
    public long TotalCount { get; set; }
    public int TotalPage { get; set; }
}
