using Dapper;

namespace Service.ContextHelpers;

public interface IDapperContext : IDisposable
{
    Task<T> GetAsync<T>(string query);
    Task<IReadOnlyList<T>> GetAllAsync<T>(string query);
    T Get<T>(string query);
    List<T> GetAll<T>(string query);
    SqlMapper.GridReader ExecuteStoredProcedure(string spName,object param=null);
    List<T> ExecuteQuery<T>(string Query,object param = null);
    int ExecuteNonReturnQuery(string Query,object param = null);

    Task<SqlMapper.GridReader> ExecuteStoredProcedureAsync(string spName, object param = null);
    Task<List<T>> ExecuteQueryAsync<T>(string Query, object param = null);
    Task<int> ExecuteNonReturnQueryAsync(string Query, object param = null);
}