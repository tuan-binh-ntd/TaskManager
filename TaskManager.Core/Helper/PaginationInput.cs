namespace TaskManager.Core.Helper
{
    public sealed class PaginationInput
    {
        public int PageNum { get; set; } = default;
        public int PageSize { get; set; } = 10;
    }
}
