namespace TaskManager.Core.DTOs;

public class SprintFilterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class EpicFilterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TypeFilterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class LabelFilterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}