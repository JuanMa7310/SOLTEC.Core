using System.Data;
using System.Data.SqlClient;
using Dapper;
using SOLTEC.Core.DataSources.Exceptions;
using SOLTEC.Core.DataSources.Interfaces;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Extensions;
using static Dapper.SqlMapper;

namespace SOLTEC.Core.DataSources;

public class SQLDataBase : IDataSource
{
    private const int _maxTimeOut = 300;

    private SqlCommand _command;
    private SqlConnection _sqlConnection;
    private bool _useTransaction;
    private SqlTransaction? _transaction;

    public string ConnectionConfig { get; set; }

    public SQLDataBase()
    {
        _command = new SqlCommand();
        _sqlConnection = new SqlConnection();
        ConnectionConfig = string.Empty;
    }

    public void CreateConnection(string connectionConfig)
    {
        try
        {
            _sqlConnection = new SqlConnection(connectionConfig);
            if (_sqlConnection?.State == ConnectionState.Closed) 
                _sqlConnection.Open();
            _command = _sqlConnection!.CreateCommand();
        }
        catch (Exception)
        {
            Dispose();
            throw new SQLException(SQLErrorEnum.Conexion);
        }
    }

    public void Dispose()
    {
        _command?.Dispose();
        _transaction?.Dispose();
        if (_sqlConnection != null)
        {
            if (_sqlConnection.State != ConnectionState.Closed) 
                _sqlConnection.Close();
            SqlConnection.ClearPool(_sqlConnection);
            _sqlConnection.Dispose();
        }
    }

    public void Dispose(SqlConnection connection, SqlCommand? command, SqlTransaction? transaction = null)
    {
        command?.Dispose();
        transaction?.Dispose();
        if (connection != null)
        {
            if (connection.State != ConnectionState.Closed) 
                connection.Close();
            connection.Dispose();
        }
    }

    public void BeginTransaction()
    {
        _useTransaction = true;
        _transaction = _sqlConnection.BeginTransaction();
        _command.Transaction = _transaction;
    }

    public void CommitTransaction()
    {
        if (_useTransaction) 
            _transaction!.Commit();
        Dispose();
    }

    public void RollbackTransaction()
    {
        if (_useTransaction)
        {
            void Run(Action act)
            {
                try
                {
                    act();
                }
                catch (Exception) { }
            }
            Run(() => _command.Dispose());
            Run(() => _transaction!.Rollback());
            Run(() => _transaction!.Dispose());
            Run(() => _sqlConnection.Close());
            Run(() => _sqlConnection.Dispose());
        }
        Dispose();
    }

    public T ExecuteDapperScalar<T>(string query, object? parameters = null, int? timeOut = null)
    {
        var result = SqlMapper.Query<T>(_sqlConnection, query, parameters, _transaction, commandTimeout: timeOut);
        return result != null ? result.FirstOrDefault() : default;
    }

    public async Task<T> ExecuteDapperScalarAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        var result = await SqlMapper.QueryAsync<T>(_sqlConnection, query, parameters, _transaction, timeOut);
        return result != null ? result.FirstOrDefault() : default;
    }

    public async Task<T?> ExecuteDapperScalarNullableAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        var result = await SqlMapper.QueryAsync<T>(_sqlConnection, query, parameters, _transaction, timeOut);
        return result.FirstOrDefault();
    }

    public IList<T> ExecuteDapper<T>(string query, object? parameters = null, int? timeOut = null)
    {
        return (IList<T>)SqlMapper.Query<T>(_sqlConnection, query, parameters, _transaction, true, timeOut);
    }

    public async Task<IList<T>> ExecuteDapperAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        return (IList<T>)await SqlMapper.QueryAsync<T>(_sqlConnection, query, parameters, _transaction, timeOut);
    }

    public GridReader ExecuteDapperMultipleQuery(string query, object? parameters = null, int? timeOut = null)
    {
        return _sqlConnection.QueryMultiple(query, parameters, _transaction, timeOut);
    }

    public async Task<GridReader> ExecuteDapperMultipleQueryAsync(string query, object? parameters = null, int? timeOut = null)
    {
        return await _sqlConnection.QueryMultipleAsync(query, parameters, _transaction, timeOut);
    }

    public async Task ExecuteConnectionAsync(string query, object? parameters = null, int? timeOut = null)
    {
        await _sqlConnection.ExecuteAsync(query, parameters, _transaction, timeOut);
    }

    public void ExecuteConnection(string query, object? parameters = null, int? timeOut = null)
    {
        _sqlConnection.Execute(query, parameters, _transaction, timeOut);
    }

    public void ExecuteQuery(string query)
    {
        try
        {
            _command.CommandText = query;
            _command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw new SQLException(SQLErrorEnum.ExecutionSql);
        }
    }

    public async Task ExecuteQueryAsync(string query)
    {
        try
        {
            _command.CommandText = query;
            await _command.ExecuteNonQueryAsync();
        }
        catch (Exception)
        {
            throw new SQLException(SQLErrorEnum.ExecutionSqlAsync);
        }
    }

    public IList<T> Select<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        connection.Open();
        var command = connection.CreateCommand();
        var result = (IList<T>)SqlMapper.Query<T>(connection, query, parameters, _transaction, true, timeOut);
        Dispose(connection, command);
        return result;
    }

    public async Task<IList<T>> SelectAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        var result = (IList<T>)await SqlMapper.QueryAsync<T>(connection, query, parameters, null, timeOut);
        Dispose(connection, command);
        command = null;
        return result;
    }

    public T? SelectScalar<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        connection.Open();
        var command = connection.CreateCommand();
        var result = SqlMapper.Query<T>(connection, query, parameters, null, commandTimeout: timeOut);
        Dispose(connection, command);
        return result != null ? result.FirstOrDefault() : default;
    }

    public async Task<T?> SelectScalarAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        var result = await SqlMapper.QueryAsync<T>(connection, query, parameters, null, commandTimeout: timeOut);
        Dispose(connection, command);
        return result != null ? result.FirstOrDefault() : default;
    }

    public GridReader SelectMultipleQuery(string query, object? parameters = null, int? timeOut = null)
    {
        GridReader result;
        using (SqlConnection connection = new(ConnectionConfig))
        {
            connection.Open();
            var command = connection.CreateCommand();
            result = connection.QueryMultiple(query, parameters, null, timeOut);
            Dispose(connection, command);
        }
        return result;
    }

    public async Task<GridReader> SelectMultipleQueryAsync(string query, object? parameters = null, int? timeOut = null)
    {
        GridReader result;
        using (SqlConnection connection = new(ConnectionConfig))
        {
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            result = await connection.QueryMultipleAsync(query, parameters, null, timeOut);
            Dispose(connection, command);
        }
        return result;
    }


    public T TransactionalQuery<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        SqlTransaction? transaction = null;
        try
        {
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            var result = (T)SqlMapper.Query<T>(connection, query, parameters, transaction, commandTimeout: timeOut);
            transaction.Commit();
            Dispose(connection, command, transaction);
            return result;
        }
        catch (Exception)
        {
            transaction?.Rollback();
            Dispose(connection, command, transaction);
            throw;
        }
    }

    public T? TransactionalQueryScalar<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        SqlTransaction? transaction = null;
        try
        {
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            var result = SqlMapper.Query<T>(connection, query, parameters, transaction, commandTimeout: timeOut);
            transaction.Commit();
            Dispose(connection, command, transaction);
            return result != null ? result.FirstOrDefault() : default;
        }
        catch (Exception)
        {
            transaction?.Rollback();
            Dispose(connection, command, transaction);
            throw;
        }
    }

    public async Task<T> TransactionalQueryAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        SqlTransaction? transaction = null;
        try
        {
            await connection.OpenAsync();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            var result = (T)await SqlMapper.QueryAsync<T>(connection, query, parameters, transaction, timeOut);
            transaction.Commit();
            Dispose(connection, command, transaction);
            return result;
        }
        catch (Exception)
        {
            transaction?.Rollback();
            Dispose(connection, command, transaction);
            throw;
        }
    }

    public async Task<T?> TransactionalQueryScalarAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        SqlTransaction? transaction = null;
        try
        {
            await connection.OpenAsync();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            var result = await SqlMapper.QueryAsync<T>(connection, query, parameters, transaction, timeOut);
            transaction.Commit();
            Dispose(connection, command, transaction);
            return result != null ? result.FirstOrDefault() : default;
        }
        catch (Exception)
        {
            transaction?.Rollback();
            Dispose(connection, command, transaction);
            throw;
        }
    }

    public GridReader TransactionalMultipleQuery(string query, object? parameters = null, int? timeOut = null)
    {
        GridReader result;
        using (SqlConnection connection = new(ConnectionConfig))
        {
            SqlCommand? command = null;
            SqlTransaction? transaction = null;
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                result = connection.QueryMultiple(query, parameters, transaction, timeOut);
                transaction.Commit();
                Dispose(connection, command, transaction);
            }
            catch (Exception)
            {
                transaction?.Rollback();
                Dispose(connection, command, transaction);
                throw;
            }
        }
        return result;
    }

    public async Task<GridReader> TransactionalMultipleQueryAsync(string query, object? parameters = null, int? timeOut = null)
    {
        GridReader result;
        using (SqlConnection connection = new(ConnectionConfig))
        {
            SqlCommand? command = null;
            SqlTransaction? transaction = null;
            try
            {
                await connection.OpenAsync();
                command = connection.CreateCommand();
                transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                result = await connection.QueryMultipleAsync(query, parameters, transaction, timeOut);
                transaction.Commit();
                Dispose(connection, command, transaction);
            }
            catch (Exception)
            {
                transaction?.Rollback();
                Dispose(connection, command, transaction);
                throw;
            }
        }
        return result;
    }

    public void Execute(string query, object? parameters = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        try
        {
            connection.Open();
            command = connection.CreateCommand();
            connection.Execute(query, parameters);
            Dispose(connection, command);
        }
        catch (Exception)
        {
            Dispose(connection, command);
            throw;
        }
    }

    public async Task ExecuteAsync(string query, object? parameters = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        try
        {
            await connection.OpenAsync();
            command = connection.CreateCommand();
            await connection.ExecuteAsync(query, parameters);
            Dispose(connection, command);
        }
        catch (Exception)
        {
            Dispose(connection, command);
            throw;
        }
    }

    public void TransactionalExecute(string query, object? parameters = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        SqlTransaction? transaction = null;
        try
        {
            connection.Open();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            connection.Execute(query, parameters, transaction);
            transaction.Commit();
            Dispose(connection, command, transaction);
        }
        catch (Exception)
        {
            transaction?.Rollback();
            Dispose(connection, command, transaction);
            throw;
        }
    }

    public async Task TransactionalExecuteAsync(string query, object? parameters = null)
    {
        using SqlConnection connection = new(ConnectionConfig);
        SqlCommand? command = null;
        SqlTransaction? transaction = null;
        try
        {
            await connection.OpenAsync();
            command = connection.CreateCommand();
            transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            await connection.ExecuteAsync(query, parameters, transaction);
            transaction.Commit();
            Dispose(connection, command, transaction);
        }
        catch (Exception)
        {
            transaction?.Rollback();
            Dispose(connection, command, transaction);
            throw;
        }
    }

    public void BulkInsert<T>(string targetTable, IEnumerable<T> data)
    {
        try
        {
            using var connection = new SqlConnection($"{ConnectionConfig};Connection Timeout={_maxTimeOut}");
            using SqlBulkCopy bulkCopy = new(connection);
            connection.Open();
            bulkCopy.BulkCopyTimeout = _maxTimeOut;
            bulkCopy.DestinationTableName = targetTable;
            bulkCopy.WriteToServer(data.ToDataTable());
        }
        catch (SQLException)
        {
            throw;
        }
    }

    public async Task BulkInsertAsync<T>(string targetTable, IEnumerable<T> data)
    {
        try
        {
            using var connection = new SqlConnection($"{ ConnectionConfig };Connection Timeout={_maxTimeOut}");
            using SqlBulkCopy bulkCopy = new(connection);
            await connection.OpenAsync();
            bulkCopy.BulkCopyTimeout = _maxTimeOut;
            bulkCopy.DestinationTableName = targetTable;
            await bulkCopy.WriteToServerAsync(data.ToDataTable());
        }
        catch (SQLException)
        {
            throw;
        }
    }
}
