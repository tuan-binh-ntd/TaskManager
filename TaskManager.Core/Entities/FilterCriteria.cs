using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class FilterCriteria : BaseEntity
{
    public string Configuration { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;

    //Relationship
    public Guid FilterId { get; set; }
    public Filter? Filter { get; set; }
    public Guid CriteriaId { get; set; }
    public Criteria? Criteria { get; set; }
}

public record MoreThan(int Quantity, string Unit);
public record Between(DateTime StartDate, DateTime EndDate);

public class ProjectCriteria
{
    public IReadOnlyCollection<Guid>? ProjectIds { get; set; }
    public IReadOnlyCollection<Guid>? SprintIds { get; set; }
    public IReadOnlyCollection<Guid>? BacklogIds { get; set; }
}

public class TypeCriteria
{
    public IReadOnlyCollection<Guid>? IssueTypeIds { get; set; }
}

public class StatusCriteria
{
    public IReadOnlyCollection<Guid>? StatusIds { get; set; }
}

public class AssigneeCriteria
{
    public Guid? CurrentUserId { get; set; }
    public bool Unassigned { get; set; }
    public IReadOnlyCollection<Guid>? UserIds { get; set; }
}

public class CreatedCriteria
{
    public MoreThan? MoreThan { get; set; }
    public Between? Between { get; set; }
}

public class DueDateCriteria
{
    public MoreThan? MoreThan { get; set; }
    public Between? Between { get; set; }
}

public class FixVersionsCriteria
{
    public bool NoVersion { get; set; }
    public IReadOnlyCollection<Guid>? VersionIds { get; set; }
}

public class LabelsCriteria
{
    public IReadOnlyCollection<Guid>? LabelIds { get; set; }
}

public class PriorityCriteria
{
    public IReadOnlyCollection<Guid>? PriorityIds { get; set; }
}

public class ReporterCriteria
{
    public Guid? CurrentUserId { get; set; }
    public bool Unassigned { get; set; }
    public IReadOnlyCollection<Guid>? UserIds { get; set; }
}

public class ResolutionCriteria
{
    public bool Unresolved { get; set; }
    public bool Done { get; set; }
}

public class ResolvedCriteria
{
    public MoreThan? MoreThan { get; set; }
    public Between? Between { get; set; }
}

public class SprintCriteria
{
    public bool NoSprint { get; set; }
    public IReadOnlyCollection<Guid>? SprintIds { get; set; }
}

public class StatusCategoryCriteria
{
    public IReadOnlyCollection<Guid>? StatusCategoryIds { get; set; }
}

public class UpdatedCriteria
{
    public MoreThan? MoreThan { get; set; }
    public Between? Between { get; set; }
}
