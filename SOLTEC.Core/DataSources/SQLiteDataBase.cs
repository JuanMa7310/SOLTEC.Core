using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using SOLTEC.Core.Exceptions;
using SOLTEC.Core.DataSources.Interfaces;
using SOLTEC.Core.Enums;
using static Dapper.SqlMapper;

namespace SOLTEC.Core.DataSources;

public class SQLiteDataBase : IDataSource
{
    private SqliteCommand _command;
    private SqliteConnection _sqlConnection;
    private bool _useTransaction;
    private SqliteTransaction? _transaction;

    public string ConnectionConfig { get; set; }

    public SQLiteDataBase()
    {
        _command = new();
        _sqlConnection = new();
        ConnectionConfig = string.Empty;
    }

    public void CreateConnection(string connectionString)
    {
        try
        {
            _sqlConnection = new SqliteConnection(connectionString);
            SqliteConnection sqlConnection = _sqlConnection;
            if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }

            _command = _sqlConnection.CreateCommand();
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
            if (_sqlConnection.State != 0)
            {
                _sqlConnection.Close();
            }

            SqliteConnection.ClearPool(_sqlConnection);
            _sqlConnection.Dispose();
        }
    }

    public void Dispose(SqliteConnection connection, SqliteCommand? command, SqliteTransaction? transaction = null)
    {
        command?.Dispose();
        transaction?.Dispose();
        if (connection != null)
        {
            if (connection.State != 0)
            {
                connection.Close();
            }

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
        {
            _transaction!.Commit();
        }
        Dispose();
    }

    public void RollbackTransaction()
    {
        if (_useTransaction)
        {
            Run(delegate {
                _command.Dispose();
            });
            Run(delegate {
                _transaction!.Rollback();
            });
            Run(delegate {
                _transaction!.Dispose();
            });
            Run(delegate {
                _sqlConnection.Close();
            });
            Run(delegate {
                _sqlConnection.Dispose();
            });
        }
        Dispose();
        static void Run(Action act)
        {
            try
            {
                act();
            }
            catch (Exception)
            {
            }
        }
    }

    public T ExecuteDapperScalar<T>(string query, object? parameters = null, int? timeOut = null)
    {
        IEnumerable<T> enumerable = _sqlConnection.Query<T>(query, parameters, _transaction, buffered: true, timeOut);
        if (enumerable == null)
        {
            return default;
        }

        return enumerable.FirstOrDefault()!;
    }

    public async Task<T> ExecuteDapperScalarAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        IEnumerable<T> enumerable = await _sqlConnection.QueryAsync<T>(query, parameters, _transaction, timeOut);

        return (T?)((enumerable != null) ? enumerable.FirstOrDefault() : ((object)default(T)));
    }

    public async Task<T?> ExecuteDapperScalarNullableAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        return (await _sqlConnection.QueryAsync<T>(query, parameters, _transaction, timeOut)).FirstOrDefault();
    }

    public IList<T> ExecuteDapper<T>(string query, object? parameters = null, int? timeOut = null)
    {
        return (IList<T>)_sqlConnection.Query<T>(query, parameters, _transaction, buffered: true, timeOut);
    }

    public async Task<IList<T>> ExecuteDapperAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        return (IList<T>)(await _sqlConnection.QueryAsync<T>(query, parameters, _transaction, timeOut));
    }

    public GridReader ExecuteDapperMultipleQuery(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
    }

    public Task<GridReader> ExecuteDapperMultipleQueryAsync(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
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
        using SqliteConnection sqlConnection = new(ConnectionConfig);
        sqlConnection.Open();
        SqliteCommand command = sqlConnection.CreateCommand();
        IList<T> result = (IList<T>)sqlConnection.Query<T>(query, parameters, _transaction, buffered: true, timeOut);
        Dispose(sqlConnection, command);

        return result;
    }

    public async Task<IList<T>> SelectAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqliteConnection connection = new(ConnectionConfig);
        await connection.OpenAsync();
        SqliteCommand command = connection.CreateCommand();
        IList<T> result = (IList<T>)(await connection.QueryAsync<T>(query, parameters, null, timeOut));
        Dispose(connection, command);
        return result;
    }

    public T? SelectScalar<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqliteConnection sqlConnection = new(ConnectionConfig);
        sqlConnection.Open();
        var command = sqlConnection.CreateCommand();
        var enumerable = sqlConnection.Query<T>(query, parameters, null, buffered: true, timeOut);
        Dispose(sqlConnection, command);
        return (T?)((enumerable != null) ? enumerable.FirstOrDefault() : ((object)default(T)!));
    }

    public async Task<T?> SelectScalarAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqliteConnection connection = new(ConnectionConfig);
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        var result = await SqlMapper.QueryAsync<T>(connection, query, parameters, null, commandTimeout: timeOut);
        Dispose(connection, command);
        return result != null ? result.FirstOrDefault() : default;
    }

    public GridReader SelectMultipleQuery(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
    }

    public Task<GridReader> SelectMultipleQueryAsync(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
    }


    public T TransactionalQuery<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqliteConnection sqlConnection = new(ConnectionConfig);
        SqliteCommand sqlCommand = new();
        SqliteTransaction? sqlTransaction = null;
        try
        {
            sqlConnection.Open();
            sqlCommand = sqlConnection.CreateCommand();
            sqlTransaction = (sqlCommand.Transaction = sqlConnection.BeginTransaction());
            T result = (T)sqlConnection.Query<T>(query, parameters, sqlTransaction, buffered: true, timeOut);
            sqlTransaction.Commit();
            Dispose(sqlConnection, sqlCommand, sqlTransaction);
            return result;
        }
        catch (Exception)
        {
            sqlTransaction?.Rollback();
            Dispose(sqlConnection, sqlCommand, sqlTransaction);
            throw;
        }
    }

    public T? TransactionalQueryScalar<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqliteConnection sqlConnection = new(ConnectionConfig);
        SqliteCommand sqlCommand = new(); 
        SqliteTransaction? sqlTransaction = null;
        try
        {
            sqlConnection.Open();
            sqlCommand = sqlConnection.CreateCommand();
            sqlTransaction = (sqlCommand.Transaction = sqlConnection.BeginTransaction());
            IEnumerable<T> enumerable = sqlConnection.Query<T>(query, parameters, sqlTransaction, buffered: true, timeOut);
            sqlTransaction.Commit();
            Dispose(sqlConnection, sqlCommand, sqlTransaction);
            return (T?)((enumerable != null) ? enumerable.FirstOrDefault() : ((object)default(T)));
        }
        catch (Exception)
        {
            sqlTransaction?.Rollback();
            Dispose(sqlConnection, sqlCommand, sqlTransaction);
            throw;
        }
    }

    public async Task<T> TransactionalQueryAsync<T>(string query, object? parameters = null, int? timeOut = null)
    {
        using SqliteConnection connection = new(ConnectionConfig);
        SqliteCommand command = new();
        SqliteTransaction? transaction = null;
        try
        {
            await connection.OpenAsync();
            command = connection.CreateCommand();
            transaction = (command.Transaction = connection.BeginTransaction());
            T result = (T)(await connection.QueryAsync<T>(query, parameters, transaction, timeOut));
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
        using SqliteConnection connection = new(ConnectionConfig);
        SqliteCommand command = new();
        SqliteTransaction? transaction = null;
        try
        {
            await connection.OpenAsync();
            command = connection.CreateCommand();
            transaction = (command.Transaction = connection.BeginTransaction());
            IEnumerable<T> enumerable = await connection.QueryAsync<T>(query, parameters, transaction, timeOut);
            transaction.Commit();
            Dispose(connection, command, transaction);
            return (T?)((enumerable != null) ? enumerable.FirstOrDefault() : ((object)default(T)));
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
        throw new NotImplementedException();
    }

    public Task<GridReader> TransactionalMultipleQueryAsync(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
    }

    public void Execute(string query, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(string query, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public void TransactionalExecute(string query, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public Task TransactionalExecuteAsync(string query, object? parameters = null)
    {
        throw new NotImplementedException();
    }

    public void BulkInsert<T>(string targetTable, IEnumerable<T> data)
    {
        throw new NotImplementedException();
    }

    public Task BulkInsertAsync<T>(string targetTable, IEnumerable<T> data)
    {
        throw new NotImplementedException();
    }

    public void ExecuteConnection(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteConnectionAsync(string query, object? parameters = null, int? timeOut = null)
    {
        throw new NotImplementedException();
    }
}
