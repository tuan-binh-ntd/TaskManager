namespace TaskManager.Core.Helper;

public class PaginationInput
{
#pragma warning disable IDE1006 // Naming Styles
    public int pagenum { get; set; } = default;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
    public int pagesize { get; set; } = default;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
    public string sort { get; set; } = string.Empty;
#pragma warning restore IDE1006 // Naming Styles

    public bool IsPaging()
    {
        return pagenum is not default(int) && pagesize is not default(int);
    }
}
