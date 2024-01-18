using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace TaskManager.Infrastructure.Repositories;

public class SqlConnectionFactory : IConnectionFactory
{
    private readonly ConnectionStrings _connectionStrings;

    public SqlConnectionFactory(IOptionsMonitor<ConnectionStrings> optionsMonitor)
    {
        _connectionStrings = optionsMonitor.CurrentValue;
    }

    public DbConnection GetDbConnection()
    {
        return new SqlConnection(_connectionStrings.DefaultConnection);
    }

    public async Task<IReadOnlyCollection<T>> QueryAsync<T>(string query, object? param = null, CommandType commandType = CommandType.Text)
    {
        using IDbConnection connection = GetDbConnection();
        var result = await connection.QueryAsync<T>(query, param, commandType: commandType);
        return result.ToList().AsReadOnly();
    }
}
