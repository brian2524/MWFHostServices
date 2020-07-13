using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

/*  Dapper functions notes
         *  Query: Use when loading data since you will probably want the rows returned to you
         *  QueryMultiple: Used for when you need multiple result sets. Returns a GridReader and can be used to return the result of multiple Select statements, using a concept of MARS (Multiple active result set).
         *  Execute: Used for executing the DML statements (like Insert, Update and Delete) whose purpose is to make changes to the data in the database. The return type is an integer.*/

namespace MultiplayerFrameworkDataLibrary.DataAccess
{
    //  Make sure to not call Load/Modify Data within a loop. This will open and close a IDbConnection every call which is expensive.
    //  Call it once and pass in a storedProcedureName that accepts a table value parameter so that you can create the table value 
    //  parameter before you call the method and pass it in as a stored procedure parameter. This allows for one database call.

    //  Make this class internal to ensure only classes within this project can call on it. When using MultiplayerFrameworkDataLibrary to contact the db you should not call on this class.
    internal static class SqlDataAccess
    {
        /*public static IEnumerable<T> LoadData<T>(string connString, string sql)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                IEnumerable<T> rows = cnn.Query<T>(sql);
                return rows;
            }
        }*/

        /*public static int ModifyDatabase<T>(string connString, string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                return cnn.Execute(sql, data);
            }
        }*/







        //  90% of loading data will call this. However if you want a stored procedure with output parameters, the caller
        //  will need to know about Dapper since it will need to use the dynamic parameters type 
        public static IEnumerable<T> LoadData<T/*, U*/>(string connString, string storedProcedureName, object/*U*/ parameters)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                IEnumerable<T> rows = cnn.Query<T>(storedProcedureName, parameters, commandType:  CommandType.StoredProcedure);
                return rows;
            }
        }

        //  I should make a more advanced version eventually that is async to save some time
        public static void SaveData/*<T>*/(string connString, string storedProcedureName, object/*T*/ parameters)
        {
            using (IDbConnection cnn = new SqlConnection(connString))
            {
                cnn.Execute(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
