using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System.Data;

namespace Hospital.SharedKernel.Infrastructure.Databases.Dapper
{
    public interface IDbConnection
    {
        SqlConnection Connection { get; }
        SqlTransaction CurrentTransaction { get; }
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

        /// <summary>
        /// Execute a single-row query asynchronously using Task.
        /// </summary>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

        /// <summary>
        ///  Execute a single-row query asynchronously using Task.
        /// </summary>
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text);

        /// <summary>
        /// Execute a command asynchronously using Task.
        /// </summary>
        /// <returns> The number of rows affected. </returns>
        Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false);

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <returns> The first cell selected as System.Object. </returns>
        Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false);

        Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false);

        /// <summary>
        /// Asynchronously copies all rows in the supplied <see cref="DataTable"/> to the destination table specified by the
        /// <see cref="DestinationTableName"/> property of the <see cref="MySqlBulkCopy"/> object.
        /// </summary>
        /// <param name="dataTable">The <see cref="DataTable"/> to copy.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="MySqlBulkCopyResult"/> with the result of the bulk copy operation.</returns>
    }
}
