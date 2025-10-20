using Microsoft.Data.SqlClient;
using System.Data;

namespace Core.Utils.MicroOrm
{
    public interface IFaraConnection
    {
        DataTable ExecuteDataTable(string tableName = "Table1");
        IEnumerable<T> ExecuteEnumerable<T>();
        List<T> ExecuteList<T>();
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync();
        SqlDataReader ExecuteReader();
        Task<SqlDataReader> ExecuteReaderAsync();
        T ExecuteScalar<T>();
        Task<T> ExecuteScalarAsync<T>();
        FaraConnection Param(string name, object value);
        FaraConnection SetCommand(string CommandText);
        FaraConnection SetSpCommand(string CommandText, int timeout = 30);
    }
}