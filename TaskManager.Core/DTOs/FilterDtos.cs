using TaskManager.Core.Core;
using TaskManager.Core.Entities;

namespace TaskManager.Core.DTOs;

public class CreateFilterDto
{
    public string Name { get; set; } = string.Empty;
    public bool Stared { get; set; } = true;
    public ProjectCriteria? Project { get; set; }
    public TypeCriteria? Type { get; set; }
    public StatusCriteria? Status { get; set; }
    public AssigneeCriteria? Assginee { get; set; }
    public CreatedCriteria? Created { get; set; }
    public DueDateCriteria? DueDate { get; set; }
    public FixVersionsCriteria? FixVersions { get; set; }
    public LabelsCriteria? Labels { get; set; }
    public PriorityCriteria? Priority { get; set; }
    public ReporterCriteria? Reporter { get; set; }
    public ResolutionCriteria? Resolution { get; set; }
    public ResolvedCriteria? Resolved { get; set; }
    public SprintCriteria? Sprints { get; set; }
    public StatusCategoryCriteria? StatusCategory { get; set; }
    public UpdatedCriteria? Updated { get; set; }
}

public class FilterConfiguration
{
    public ProjectCriteria? Project { get; set; }
    public TypeCriteria? Type { get; set; }
    public StatusCriteria? Status { get; set; }
    public AssigneeCriteria? Assginee { get; set; }
    public CreatedCriteria? Created { get; set; }
    public DueDateCriteria? DueDate { get; set; }
    public FixVersionsCriteria? FixVersions { get; set; }
    public LabelsCriteria? Labels { get; set; }
    public PriorityCriteria? Priority { get; set; }
    public ReporterCriteria? Reporter { get; set; }
    public ResolutionCriteria? Resolution { get; set; }
    public ResolvedCriteria? Resolved { get; set; }
    public SprintCriteria? Sprints { get; set; }
    public StatusCategoryCriteria? StatusCategory { get; set; }
    public UpdatedCriteria? Updated { get; set; }

    //public string ProjectCriteria()
    //{

    //}

    public string IssueTypeCriteriaQuery()
    {
        if (Type?.IssueTypeIds is not null && Type.IssueTypeIds.Any())
        {
            return @$"
                AND
                IssueTypeId IN ({string.Join(",", Type.IssueTypeIds.Select(x => $"'{x}'"))})
            ";
        }

        return string.Empty;
    }

    public string StatusCriteriaQuery()
    {
        if (Status?.StatusIds is not null && Status.StatusIds.Any())
        {
            return @$"
                AND
                StatusId IN ({string.Join(",", Status.StatusIds.Select(x => $"'{x}'"))})
            ";
        }

        return string.Empty;
    }

    public string AssigneeCriteriaQuery()
    {
        string query = string.Empty;
        if (Assginee is null)
        {
            return string.Empty;
        }
        if (Assginee?.UserIds is not null && Assginee.UserIds.Any())
        {
            query = @$"
                AND
                AssigneeId IN ({string.Join(",", Assginee.UserIds.Select(x => $"'{x}'"))})
            ";
        }
        if (Assginee?.CurrentUserId is Guid currentUserId)
        {
            query += @$"
                OR
                AssigneeId = '{currentUserId}'
            ";
        }
        if (Assginee is not null && Assginee.Unassigned)
        {
            query += @$"
                OR
                AssigneeId = NULL
            ";
        }
        return query;
    }

    public string CreatedCriteriaQuery()
    {
        string query = "AND";
        if (Created is null)
        {
            return string.Empty;
        }
        else if (Created?.MoreThan is MoreThan moreThan)
        {
            if (moreThan.Unit.Equals(CoreConstants.MinutesUnit))
            {
                query = @$"
                    DATEDIFF(MINUTE , CreationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.HoursUnit))
            {
                query = @$"
                    DATEDIFF(HOUR , CreationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.DaysUnit))
            {
                query = @$"
                    DATEDIFF(DAY , CreationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.WeekUnit))
            {
                query = @$"
                    DATEDIFF(WEEK , CreationTime, GETDATE())
                ";
            }
            return $"{query} = {moreThan.Quantity}";
        }
        else if (Created?.Between is Between between)
        {
            query = $@"
                CreationTime BETWEEN {between.StartDate} AND {between.EndDate} 
            ";
            return query;
        }
        return query;
    }

    public string DueDateCriteriaQuery()
    {
        string query = "AND";
        if (DueDate is null)
        {
            return string.Empty;
        }
        else if (DueDate?.MoreThan is MoreThan moreThan)
        {
            if (moreThan.Unit.Equals(CoreConstants.MinutesUnit))
            {
                query = @$"
                    DATEDIFF(MINUTE , DueDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.HoursUnit))
            {
                query = @$"
                    DATEDIFF(HOUR , DueDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.DaysUnit))
            {
                query = @$"
                    DATEDIFF(DAY , DueDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.WeekUnit))
            {
                query = @$"
                    DATEDIFF(WEEK , DueDate, GETDATE())
                ";
            }
            return $"{query} = {moreThan.Quantity}";
        }
        else if (DueDate?.Between is Between between)
        {
            query = $@"
                DueDate BETWEEN {between.StartDate} AND {between.EndDate} 
            ";
            return query;
        }
        return query;
    }

    public string FixVersionsCriteriaQuery()
    {
        string query = "AND";
        if (FixVersions is null)
        {
            return string.Empty;
        }
        if (FixVersions.NoVersion)
        {
            query += $@"
                
            ";
        }
        if (FixVersions.VersionIds is not null && FixVersions.VersionIds.Any())
        {
            query += $@"
                VersionId IN ({string.Join(",", FixVersions.VersionIds.Select(x => $"'{x}'"))})
            ";
        }
        return query;
    }

    public string LabelsCriteriaQuery()
    {
        string query = "AND";
        if (Labels is null)
        {
            return string.Empty;
        }
        if (Labels.LabelIds is not null && Labels.LabelIds.Any())
        {
            query += $@"
                Labels IN ({string.Join(",", Labels.LabelIds.Select(x => $"'{x}'"))})
            ";
        }
        return query;
    }

    public string PriorityCriteriaQuery()
    {
        string query = "AND";
        if (Priority is null)
        {
            return string.Empty;
        }
        if (Priority.PriorityIds is not null && Priority.PriorityIds.Any())
        {
            query += $@"
                PriorityId IN ({string.Join(",", Priority.PriorityIds.Select(x => $"'{x}'"))})
            ";
        }
        return query;
    }

    public string ReporterCriteriaQuery()
    {
        string query = "AND";
        if (Reporter is null)
        {
            return string.Empty;
        }
        if (Reporter?.UserIds is not null && Reporter.UserIds.Any())
        {
            query = @$"
                AND
                ReporterId IN ({string.Join(",", Reporter.UserIds.Select(x => $"'{x}'"))})
            ";
        }
        if (Reporter?.CurrentUserId is Guid currentUserId)
        {
            query += @$"
                OR
                ReporterId = '{currentUserId}'
            ";
        }
        if (Reporter is not null && Reporter.Unassigned)
        {
            query += @$"
                OR
                ReporterId = NULL
            ";
        }
        return query;
    }

    //public string ResolutionCriteriaQuery()
    //{
    //    string query = "AND";
    //    if (Resolution is null)
    //    {
    //        return string.Empty;
    //    }
    //    if (Resolution.Unresolved)
    //    {
    //        query += $@"
    //            sc.Code <> 'Done'
    //        ";
    //    }
    //    if (Resolution.Done)
    //    {
    //        query += $@"
    //            sc.Code = 'Done'
    //        ";
    //    }
    //}

}
