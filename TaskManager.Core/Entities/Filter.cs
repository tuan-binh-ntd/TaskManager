using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class Filter : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool Stared { get; set; } = true;
    public string Type { get; set; } = string.Empty;
    public string Configuration { get; set; } = string.Empty;

    //Relationship
    public ICollection<UserFilter>? UserFilters { get; set; }
    public ICollection<FilterCriteria>? FilterCriterias { get; set; }
}
