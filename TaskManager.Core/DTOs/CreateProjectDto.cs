namespace TaskManager.Core.DTOs;

public class CreateProjectDto
{
    [Required]
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [Required]
    public string? Code { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } = string.Empty;
    public bool IsFavourite { get; set; } = false;
}



public class GetIssueForProjectFilterInputModel
{
    public IReadOnlyCollection<Guid> EpicIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid> LabelIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid> IssueTypeIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid> VerionIds { get; set; } = new List<Guid>();
    public string? SearchKey { get; set; } = string.Empty;
}

public class GetIssueForProjectFilterDto : GetIssueForProjectFilterInputModel
{
    public Guid BacklogId { get; set; }

    private static string BaseQuery()
    {
        return @"
            SELECT 
            DISTINCT
              i.Id
            FROM IssueTypes it
            JOIN Issues i ON it.Id = i.IssueTypeId AND it.[Level] = 2 AND it.ProjectId IS NOT NULL
            LEFT JOIN LabelIssues li ON i.Id = li.IssueId
            LEFT JOIN VersionIssues vi ON i.Id = vi.IssueId
            WHERE 1 = 1
        ";
    }

    private string BuildSubWhereQuery()
    {
        string query = "AND";
        List<string> querys = new();

        if (EpicIds.Any())
        {
            string epicQuery = $@"
                ParentId IN ({string.Join(",", EpicIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(epicQuery);
        }
        if (LabelIds.Any())
        {
            string labelQuery = @$"
                LabelId IN ({string.Join(",", LabelIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(labelQuery);
        }
        if (IssueTypeIds.Any())
        {
            string typeQuery = $@"
                IssueTypeId IN ({string.Join(",", IssueTypeIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(typeQuery);
        }
        if (VerionIds.Any())
        {
            string versionQuery = $@"
                vi.VersionId IN ({string.Join(",", VerionIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(versionQuery);
        }
        if (!string.IsNullOrWhiteSpace(SearchKey))
        {
            string searchKeyQuery = $@"
                i.[Name] LIKE '%{SearchKey}%'
            ";
            querys.Add(searchKeyQuery);
        }
        if (querys.Any())
        {
            query = $@"{query} {string.Join(" AND ", querys.Select(x => x))}";
        }
        string backlogQuery = @$"
            OR 
            i.BacklogId = '{BacklogId}'
        ";
        query += backlogQuery;
        return query.Equals("AND") ? string.Empty : query;
    }

    public string FullQuery() => BaseQuery()
        + BuildSubWhereQuery();
}