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
    public ICollection<Guid>? ProjectIds { get; set; }
}

public class TypeCriteria
{
    public ICollection<Guid>? IssueTypeIds { get; set; }
}

public class StatusCriteria
{
    public ICollection<Guid>? StatusIds { get; set; }
}

public class AssigneeCriteria
{
    public Guid? CurrentUserId { get; set; }
    public bool Unassigned { get; set; }
    public ICollection<Guid>? UserIds { get; set; }
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
    public ICollection<Guid>? VersionIds { get; set; }
}

public class LabelsCriteria
{
    public ICollection<Guid>? LabelIds { get; set; }
}

public class PriorityCriteria
{
    public ICollection<Guid>? PriorityIds { get; set; }
}

public class ReporterCriteria
{
    public Guid? CurrentUserId { get; set; }
    public bool Unassigned { get; set; }
    public ICollection<Guid>? UserIds { get; set; }
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
    public ICollection<Guid>? SprintIds { get; set; }
}

public class StatusCategoryCriteria
{
    public bool Todo { get; set; }
    public bool InProgress { get; set; }
    public bool Done { get; set; }
}

public class UpdatedCriteria
{
    public MoreThan? MoreThan { get; set; }
    public Between? Between { get; set; }
}
