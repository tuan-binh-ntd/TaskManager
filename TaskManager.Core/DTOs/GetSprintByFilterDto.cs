namespace TaskManager.Core.DTOs;

public class GetSprintByFilterDto
{
    public IReadOnlyCollection<Guid> EpicIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid> LabelIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid> IssueTypeIds { get; set; } = new List<Guid>();
    public IReadOnlyCollection<Guid> SprintIds { get; set; } = new List<Guid>();
    public string? SearchKey { get; set; } = string.Empty;

    private static string BaseQuery()
    {
        return @"
            SELECT 
            DISTINCT
              i.Id
            FROM IssueTypes it
            JOIN Issues i ON it.Id = i.IssueTypeId AND it.[Level] = 2 AND it.ProjectId IS NOT NULL
            LEFT JOIN LabelIssues li ON i.Id = li.IssueId
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
        if (SprintIds.Any())
        {
            string sprintQuery = $@"
                SprintId IN ({string.Join(",", SprintIds.Select(x => $"'{x}'"))})
            ";
            querys.Add(sprintQuery);
        }
        if (!string.IsNullOrWhiteSpace(SearchKey))
        {
            string searchKeyQuery = $@"
                i.[Name] = '{SearchKey}'
            ";
            querys.Add(searchKeyQuery);
        }
        if (querys.Any())
        {
            query = $@"{query} {string.Join(" AND ", querys.Select(x => x))}";
        }
        return query.Equals("AND") ? string.Empty : query;
    }

    public string FullQuery() => BaseQuery()
        + BuildSubWhereQuery();
}
