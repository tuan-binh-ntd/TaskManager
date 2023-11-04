using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
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

    public class AssigneeCriteria
    {
        public bool CurrentUser { get; set; }
        public bool Unassigned { get; set; }
        public ICollection<Guid>? SuggestedUsers { get; set; }
        public ICollection<Guid>? SuggestedGroups { get; set; }

        public string BuildCurrentUserSqlQuery()
        {
            if (CurrentUser is true)
            {
                return @"AssigneeId = @UserId";
            }
            else
            {
                return string.Empty;
            }
        }

        public string BuildUnassignedSqlQuery()
        {
            if (Unassigned is true)
            {
                return @"AssigneeId IS NULL";
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class ResolutionCriteria
    {
        public bool Unresolved { get; set; }
        public bool Done { get; set; }
    }

    public class ReporterCriteria
    {
        public bool CurrentUser { get; set; }
        public bool Unassigned { get; set; }
        public ICollection<Guid>? SuggestedUsers { get; set; }
        public ICollection<Guid>? SuggestedGroups { get; set; }
    }

    public class StatusCategoryCriteria
    {
        public bool Todo { get; set; }
        public bool InProgress { get; set; }
        public bool Done { get; set; }
    }

    public class CreatedCriteria
    {
        public WithinTheLast? WithinTheLast { get; set; }
        public MoreThan? MoreThan { get; set; }
        public Between? Between { get; set; }
        public InTheRange? InTheRange { get; set; }
    }

    public record WithinTheLast(int Quantity, string Unit);
    public record MoreThan(int Quantity, string Unit);
    public record Between(DateTime StartDate, DateTime EndDate);
    public record InTheRange(DateTime From, DateTime To);

    public class ResolvedCriteria
    {
        public WithinTheLast? WithinTheLast { get; set; }
        public MoreThan? MoreThan { get; set; }
        public Between? Between { get; set; }
        public InTheRange? InTheRange { get; set; }
    }

    public class UpdatedCriteria
    {
        public WithinTheLast? WithinTheLast { get; set; }
        public MoreThan? MoreThan { get; set; }
        public Between? Between { get; set; }
        public InTheRange? InTheRange { get; set; }
    }
}
