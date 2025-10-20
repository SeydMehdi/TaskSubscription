
using System.Data;
using CleanArchitecture.Common.Core.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;


/// <summary>
/// Summary description for FaraConnection
/// </summary>
/// 

namespace Core.Utils.MicroOrm
{
    public class FaraConnection : IFaraConnection, IDisposable
    {
        private SqlConnection connection;
        protected SqlCommand command;
        private SqlTransaction currentTrasaction;

        public FaraConnection(IDbConnection connection)
        {
            this.connection = (SqlConnection)connection;
            if (connection != null && connection.State != ConnectionState.Open)
                connection.Open();
        }
        public FaraConnection SetCommand(string CommandText)
        {
            this.command = connection.CreateCommand();
            command.CommandText = CommandText;
            if (currentTrasaction != null)
            {
                command.Transaction = currentTrasaction;
            }
            return this;
        }
        public FaraConnection SetSpCommand(string CommandText, int timeout = 30)
        {
            this.command = connection.CreateCommand();
            command.CommandText = CommandText;
            command.CommandTimeout = timeout;
            command.CommandType = CommandType.StoredProcedure;
            return this;
        }
        public FaraConnection Param(string name, object value)
        {
            this.command.Parameters.AddWithValue(name, value);
            return this;
        }
        public SqlDataReader ExecuteReader()
        {
            return command.ExecuteReader();

        }
        public DataTable ExecuteDataTable(string tableName = "Table1")
        {
            DataTable table = new DataTable();
            var reader = command.ExecuteReader();
            try
            {
                table.Load(reader);
            }
            catch (Exception)
            {
                reader.Close();
                throw;
            }


            return table;
        }
        public async Task<DataTable> ExecuteDataTableAsync(string tableName = "Table1")
        {
            DataTable table = new DataTable();
            var reader = await command.ExecuteReaderAsync();
            try
            {
                table.Load(reader);
            }
            catch (Exception)
            {
                reader.Close();
                throw;
            }


            return table;
        }
        public List<T> ExecuteList<T>()
        {
            var table = ExecuteDataTable();
            return table.MapToList<T>(null);
        }
        public async Task<List<T>> ExecuteListAsync<T>()
        {
            var table = await ExecuteDataTableAsync();
            return table.MapToList<T>(null);
        }
        public IEnumerable<T> ExecuteEnumerable<T>()
        {
            using (var reader = command.ExecuteReader())
            {
                return reader.MapToEnumerable<T>(null);
            }
        }
        public async Task<int> ExecuteNonQueryAsync()
        {
            return await command.ExecuteNonQueryAsync();
        }
        public int ExecuteNonQuery()
        {
            return command.ExecuteNonQuery();
        }
        public T ExecuteScalar<T>()
        {
            return (T)command.ExecuteScalar();
        }
        public async Task<T> ExecuteScalarAsync<T>()
        {
            return (T)(await command.ExecuteScalarAsync());
        }
        public async Task<SqlDataReader> ExecuteReaderAsync()
        {

            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();
            return await command.ExecuteReaderAsync();
        }
        public void Dispose()
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }


        }
        public void BeginTransaction()
        {
            this.currentTrasaction = connection.BeginTransaction();
        }
        public void CommitTransaction()
        {

            if (this.currentTrasaction != null)
            {
                this.currentTrasaction.Commit();
                this.currentTrasaction.Dispose();
                this.currentTrasaction = null;

                return;
            }
            throw new Exception("No active transaction.");
        }
        public void RollbacknTransaction()
        {
            if (this.currentTrasaction != null)
            {
                this.currentTrasaction.Rollback();
                this.currentTrasaction.Dispose();
                this.currentTrasaction = null;
                return;
            }
            throw new Exception("No active transaction.");
        }


    }
}
