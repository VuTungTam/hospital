using Dapper;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Events.BaseEvents;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using System.Data;
using static Dapper.SqlMapper;

namespace Hospital.SharedKernel.Infrastructure.Databases.Dapper
{
    public class DbConnection : IDbConnection, IUnitOfWork
    {
        #region Declare + Constructor
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;
        public DbConnection(string dbConfig = "RootDatabase")
        {
            _connection = new SqlConnection(ConnectionStringConfig.ConnectionStrings[dbConfig]);
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

        }
        #endregion
        #region Implementation
        private List<DomainEvent> DomainEvents { get; set; } = new();

        public SqlConnection Connection => _connection;

        public SqlTransaction CurrentTransaction
        {
            get
            {
                if (_transaction == null || _transaction.Connection == null)
                {
                    _transaction = _connection.BeginTransaction();
                }
                return _transaction;
            }
        }
        #region Query

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            return await _connection.QueryMultipleAsync(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }
        #endregion

        #region Command
        public async Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            var result = await _connection.ExecuteAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
            if (autoCommit)
            {
                await CommitAsync();
            }
            return result;
        }

        public async Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            if (param != null && typeof(IBaseEntity).IsAssignableFrom(param.GetType()))
            {
                var @base = (IBaseEntity)param;
                if (@base.DomainEvents != null && @base.DomainEvents.Any())
                {
                    DomainEvents.AddRange(@base.DomainEvents);
                }
            }

            var result = await _connection.ExecuteScalarAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
            if (autoCommit)
            {
                await CommitAsync();
            }
            return result;
        }

        public async Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            if (param != null && typeof(IBaseEntity).IsAssignableFrom(param.GetType()))
            {
                var @base = (IBaseEntity)param;
                if (@base.DomainEvents != null && @base.DomainEvents.Any())
                {
                    DomainEvents.AddRange(@base.DomainEvents);
                }
            }
            return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }
        #endregion
        #region UnitOfWork
        public async Task CommitAsync(bool dispatchEvent = true, CancellationToken cancellationToken = default)
        {
            await CurrentTransaction.CommitAsync(cancellationToken);
            if (!dispatchEvent)
            {
                DomainEvents.Clear();
            }
        }
        #endregion
        public IDbContextTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }
        #region Dispose
        public void Dispose()
        {
            //GC.SuppressFinalize(this);
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        #endregion
        #endregion

    }
}
