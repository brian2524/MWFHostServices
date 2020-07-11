using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MultiplayerFrameworkDataLibrary.DataAccess
{
    public static class SqlDataAccess
    {
        public static List<T> LoadData<T>(string connString, string sql)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        public static int ModifyDatabase<T>(string connString, string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Execute(sql, data);
            }
        }

        public static List<T> ExecuteProcedure<T>(string connectionString, string procedureName, object ProcedureParameters)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<T>(procedureName, ProcedureParameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

    }
}
