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
        public static List<T> LoadData<T>(string sql, string connString)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        public static int ModifyDatabase<T>(string sql, T data, string connString)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Execute(sql, data);
            }
        }

    }
}
