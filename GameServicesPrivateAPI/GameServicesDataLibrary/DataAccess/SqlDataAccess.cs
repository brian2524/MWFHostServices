using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Linq;

namespace GameServicesDataLibrary.DataAccess
{
    public class SqlDataAccess
    {
        public static List<T> LoadData<T>(string sql, string connString)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        public static int ModifyData<T>(string sql, T data, string connString)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Execute(sql, data);
            }
        }

        public static List<int> ExecuteProcedure(string procedureName, object param, string connString)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Query<int>(procedureName, param, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
