using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Core;
using TaskManager.Core.Entities;

namespace TaskManager.Core.DTOs;

public class CreateFilterDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
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
    [Required]
    public Guid CreatorUserId { get; set; }
    public string? Descrption { get; set; }
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

    private static string BaseQuery()
    {
        return @"
            SELECT 
              i.Id
            FROM (SELECT Id, Code FROM StatusCategories WHERE Code IN (N'To-do', N'In-Progress', N'Done')) sc
            INNER JOIN Statuses s ON sc.Id = s.StatusCategoryId
            INNER JOIN Issues i ON s.Id = i.StatusId
            INNER JOIN IssueDetails id ON i.Id = id.IssueId
            LEFT JOIN LabelIssues li ON i.Id = li.IssueId
            LEFT JOIN VersionIssues vi ON i.Id = vi.IssueId
            WHERE 1 = 1
        ";
    }

    private string ProjectCriteriaQuery()
    {
        string query = @"AND
            (
        ";
        List<string> querys = new();

        if (Project?.SprintIds is not null && Project.SprintIds.Any())
        {
            string sprintQuery = @$"
                SprintId IN ({string.Join(",", Project.SprintIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(sprintQuery);
        }
        if (Project?.BacklogIds is not null && Project.BacklogIds.Any())
        {
            string backlogQuery = $@"
                BacklogId IN ({string.Join(",", Project.BacklogIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(backlogQuery);
        }
        if (querys.Any())
        {
            query = $@"{query} {string.Join(" OR ", querys.Select(x => x))}
                )
            ";
        }
        return query.Equals(@"AND
            (
        ") ? string.Empty : query;
    }

    private string IssueTypeCriteriaQuery()
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

    private string StatusCriteriaQuery()
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

    private string AssigneeCriteriaQuery()
    {
        string query = "AND";
        List<string> querys = new();

        if (Assginee is null)
        {
            return string.Empty;
        }
        if (Assginee?.UserIds is not null && Assginee.UserIds.Any())
        {
            string inQuery = @$"
                AssigneeId IN ({string.Join(",", Assginee.UserIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(inQuery);
        }
        if (Assginee?.CurrentUserId is Guid currentUserId)
        {
            string equalQuery = @$"
                AssigneeId = '{currentUserId}'
            ";
            querys.Add(equalQuery);
        }
        if (Assginee is not null && Assginee.Unassigned)
        {
            string nullQuery = @$"
                AssigneeId = NULL
            ";
            querys.Add(nullQuery);
        }
        if (querys.Any())
        {
            query = $"{query} {string.Join(" OR ", querys.Select(x => x))}";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string CreatedCriteriaQuery()
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
                query += @$"
                    DATEDIFF(MINUTE , i.CreationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.HoursUnit))
            {
                query += @$"
                    DATEDIFF(HOUR , i.CreationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.DaysUnit))
            {
                query += @$"
                    DATEDIFF(DAY , i.CreationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.WeekUnit))
            {
                query += @$"
                    DATEDIFF(WEEK , i.CreationTime, GETDATE())
                ";
            }
            return $@"{query} = {moreThan.Quantity}
            ";
        }
        else if (Created?.Between is Between between)
        {
            query += $@"
                i.CreationTime BETWEEN '{between.StartDate:yyyy-MM-dd}' AND '{between.EndDate:yyyy-MM-dd}'
            ";
            return query;
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string DueDateCriteriaQuery()
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
                query += @$"
                    DATEDIFF(MINUTE , DueDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.HoursUnit))
            {
                query += @$"
                    DATEDIFF(HOUR , DueDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.DaysUnit))
            {
                query += @$"
                    DATEDIFF(DAY , DueDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.WeekUnit))
            {
                query += @$"
                    DATEDIFF(WEEK , DueDate, GETDATE())
                ";
            }
            return $@"{query} = {moreThan.Quantity}
            ";
        }
        else if (DueDate?.Between is Between between)
        {
            query += $@"
                DueDate BETWEEN '{between.StartDate:yyyy-MM-dd}' AND '{between.EndDate:yyyy-MM-dd}'
            ";
            return query;
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string FixVersionsCriteriaQuery()
    {
        string query = "AND";
        List<string> querys = new();

        if (FixVersions is null)
        {
            return string.Empty;
        }
        if (FixVersions.NoVersion)
        {
            string nullQuery = $@"
                li.VersionId = NULL
            ";
            querys.Add(nullQuery);
        }
        if (FixVersions.VersionIds is not null && FixVersions.VersionIds.Any())
        {
            string inQuery = $@"
                li.VersionId IN ({string.Join(",", FixVersions.VersionIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(inQuery);
        }
        if (querys.Any())
        {
            query = $"{query} {string.Join(" OR ", querys.Select(x => x))}";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string LabelsCriteriaQuery()
    {
        string query = "AND";
        if (Labels is null)
        {
            return string.Empty;
        }
        if (Labels.LabelIds is not null && Labels.LabelIds.Any())
        {
            query += $@"
                LabelId IN ({string.Join(",", Labels.LabelIds.Select(x => $"'{x}'"))})
            ";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string PriorityCriteriaQuery()
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
        return query.Equals("AND") ? string.Empty : query;
    }

    private string ReporterCriteriaQuery()
    {
        string query = "AND";
        List<string> querys = new();

        if (Reporter is null)
        {
            return string.Empty;
        }
        if (Reporter?.UserIds is not null && Reporter.UserIds.Any())
        {
            string inQuery = @$"
                ReporterId IN ({string.Join(",", Reporter.UserIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(inQuery);
        }
        if (Reporter?.CurrentUserId is Guid currentUserId)
        {
            string equalQuery = @$"
                ReporterId = '{currentUserId}'
            ";
            querys.Add(equalQuery);
        }
        if (querys.Any())
        {
            query = $"{query} {string.Join(" OR ", querys.Select(x => x))}";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string ResolutionCriteriaQuery()
    {
        string query = "AND";
        List<string> querys = new();

        if (Resolution is null)
        {
            return string.Empty;
        }
        if (Resolution.Unresolved)
        {
            string notEqualQuery = $@"
                sc.Code <> 'Done'
            ";
            querys.Add(notEqualQuery);
        }
        if (Resolution.Done)
        {
            string equalQuery = $@"
                sc.Code = 'Done'
            ";
            querys.Add(equalQuery);
        }
        if (querys.Any())
        {
            query = $"{query} {string.Join(" OR ", querys.Select(x => x))}";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string ResolvedCriteriaQuery()
    {
        string query = "AND";
        if (Resolved is null)
        {
            return string.Empty;
        }
        else if (Resolved?.MoreThan is MoreThan moreThan)
        {
            if (moreThan.Unit.Equals(CoreConstants.MinutesUnit))
            {
                query += @$"
                    DATEDIFF(MINUTE , CompleteDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.HoursUnit))
            {
                query += @$"
                    DATEDIFF(HOUR , CompleteDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.DaysUnit))
            {
                query += @$"
                    DATEDIFF(DAY , CompleteDate, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.WeekUnit))
            {
                query += @$"
                    DATEDIFF(WEEK , CompleteDate, GETDATE())
                ";
            }
            return $@"{query} = {moreThan.Quantity}
            ";
        }
        else if (Resolved?.Between is Between between)
        {
            query += $@"
                CompleteDate BETWEEN '{between.StartDate:yyyy-MM-dd}' AND '{between.EndDate:yyyy-MM-dd}'
            ";
            return query;
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string SprintsCriteriaQuery()
    {
        string query = "AND";
        List<string> querys = new();

        if (Sprints is null)
        {
            return string.Empty;
        }
        if (Sprints.SprintIds is not null && Sprints.SprintIds.Any())
        {
            string inQuery = @$"SprintId IN ({string.Join(",", Sprints.SprintIds.Select(x => $"'{x}'"))})";
            querys.Add(inQuery);
        }
        if (Sprints.NoSprint)
        {
            string equalQuery = @$"
                SprintId = NULL
            ";
            querys.Add(equalQuery);
        }
        if (querys.Any())
        {
            query = $"{query} {string.Join(" OR ", querys.Select(x => x))}";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string StatusCategoryCriteriaQuery()
    {
        string query = "AND";
        if (StatusCategory is null)
        {
            return string.Empty;
        }
        else if (StatusCategory.StatusCategoryIds is not null && StatusCategory.StatusCategoryIds.Any())
        {
            query += @$"
                StatusCategoryId IN ({string.Join(",", StatusCategory.StatusCategoryIds.Select(x => $"'{x}'"))})
            ";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    private string UpdatedCriteriaQuery()
    {
        string query = "AND";
        if (Updated is null)
        {
            return string.Empty;
        }
        else if (Updated?.MoreThan is MoreThan moreThan)
        {
            if (moreThan.Unit.Equals(CoreConstants.MinutesUnit))
            {
                query += @$"
                    DATEDIFF(MINUTE , i.ModificationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.HoursUnit))
            {
                query += @$"
                    DATEDIFF(HOUR , i.ModificationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.DaysUnit))
            {
                query += @$"
                    DATEDIFF(DAY , i.ModificationTime, GETDATE())
                ";
            }
            else if (moreThan.Unit.Equals(CoreConstants.WeekUnit))
            {
                query += @$"
                    DATEDIFF(WEEK , i.ModificationTime, GETDATE())
                ";
            }
            return $@"{query} = {moreThan.Quantity}
            ";
        }
        else if (Updated?.Between is Between between)
        {
            query += $@"
                i.ModificationTime BETWEEN '{between.StartDate:yyyy-MM-dd}' AND '{between.EndDate:yyyy-MM-dd}'
            ";
            return query;
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    public string QueryAfterBuild() =>
        BaseQuery()
        + ProjectCriteriaQuery()
        + IssueTypeCriteriaQuery()
        + StatusCriteriaQuery()
        + AssigneeCriteriaQuery()
        + CreatedCriteriaQuery()
        + DueDateCriteriaQuery()
        + FixVersionsCriteriaQuery()
        + LabelsCriteriaQuery()
        + PriorityCriteriaQuery()
        + ReporterCriteriaQuery()
        + ResolutionCriteriaQuery()
        + ResolvedCriteriaQuery()
        + SprintsCriteriaQuery()
        + StatusCategoryCriteriaQuery()
        + UpdatedCriteriaQuery()
        ;
}

public class GetIssueByConfigurationDto
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
}

public class UpdateFilterDto
{
    public string? Name { get; set; }
    public bool? Stared { get; set; }
    public string? Descrption { get; set; }
}