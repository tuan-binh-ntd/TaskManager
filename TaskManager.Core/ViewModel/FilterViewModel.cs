namespace TaskManager.Core.ViewModel;

public class FilterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Stared { get; set; } = true;
    public string Type { get; set; } = string.Empty;
    public FilterConfiguration Configuration { get; set; } = new FilterConfiguration();
    public string? Description { get; set; }
}
