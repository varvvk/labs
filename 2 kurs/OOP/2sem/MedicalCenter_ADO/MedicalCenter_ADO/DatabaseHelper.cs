using System.Data;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace MedicalCenter_ADO
{
    public class DatabaseHelper
    {
        public string ConnectionString { get; }

        public DatabaseHelper()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["MedicalCenterDB"].ConnectionString;
        }

        public DataTable GetAll(string tableName)
        {
            using var conn = new SqlConnection(ConnectionString);
            var dt = new DataTable();
            var query = $"SELECT * FROM {tableName}";
            var adapter = new SqlDataAdapter(query, conn);
            adapter.Fill(dt);
            return dt;
        }


        public DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddRange(parameters);

            var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

    }

}