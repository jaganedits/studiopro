using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Service.ContextHelpers;

 public class DapperContext : IDapperContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString = string.Empty;
        private readonly SqlConnection connection;

        // private readonly SqlConnection connection;
        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("Service");
            connection = new SqlConnection(connectionString);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        // Async methods  start **************************************************************************************************************

        public async Task<T> GetAsync<T>(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<T>(query);
                return result;
            }
        }
        public async Task<IReadOnlyList<T>> GetAllAsync<T>(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<T>(query);
                return result.ToList();
            }
        }

        // Async methods  end **************************************************************************************************************

        public T Get<T>(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<T>(query);
                return result;
            }
        }
        public List<T> GetAll<T>(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var result = connection.Query<T>(query);
                return result.ToList();
            }
        }

        public SqlMapper.GridReader ExecuteStoredProcedure(string spName, object param = null)
        {
             var connection = new SqlConnection(connectionString);
                // connection.OpenAsync();
             
            var multi = connection.QueryMultiple(spName, param,
                                commandType: CommandType.StoredProcedure);
            return multi;
        }

        public async Task<SqlMapper.GridReader> ExecuteStoredProcedureAsync(string spName, object param = null)
        {
             var connection = new SqlConnection(connectionString);
            //    await connection.OpenAsync();
            var multi = await connection.QueryMultipleAsync(spName, param,
                                commandType: CommandType.StoredProcedure);
            return multi;
        }

        public List<T> ExecuteQuery<T>(string Query, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var multi = connection.Query<T>(Query, param,
                                commandType: CommandType.Text).ToList();
                return multi;
            }
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string Query, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var multi = await connection.QueryAsync<T>(Query, param,
                                    commandType: CommandType.Text);
                return multi.ToList();
            }
        }

        public int ExecuteNonReturnQuery(string Query, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var multi = connection.Execute(Query, param,
                                    commandType: CommandType.Text);
                return multi;
            }
        }
        public async Task<int> ExecuteNonReturnQueryAsync(string Query, object param = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var multi = await connection.ExecuteAsync(Query, param,
                                    commandType: CommandType.Text);
                return multi;
            }
        }

        

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
        #endregion IDisposable
    }