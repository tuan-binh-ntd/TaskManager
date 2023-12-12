using System.Data;
using System.Data.Common;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IConnectionFactory
{
    DbConnection GetDbConnection();
    Task<IReadOnlyCollection<T>> QueryAsync<T>(string query, object? param = null, CommandType commandType = CommandType.Text);
}
