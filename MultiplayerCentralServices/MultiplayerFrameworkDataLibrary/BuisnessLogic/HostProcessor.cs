using MultiplayerFrameworkDataLibrary.DataAccess;
using MultiplayerFrameworkModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerFrameworkDataLibrary.BuisnessLogic
{
    class HostProcessor
    {
        public static int CreateServer(string serverIP, string gameInstancesManagementApiIp, string gameInstancesManagementApiPort, bool isActive, string connString)
        {
            var parameters = new
            {
                ServerIP = serverIP,
                GameInstancesManagementApiIp = gameInstancesManagementApiIp,
                GameInstancesManagementApiPort = gameInstancesManagementApiPort,
                IsActive = isActive
            };

            //  Old way (not using stored procedures and using older function that took in a string sql)
            /*string sql = @"insert into dbo.ServerTable (ServerIP, GameInstancesManagementApiIp, GameInstancesManagementApiPort, IsActive)
                         values (@ServerIP, @GameInstancesManagementApiIp, @GameInstancesManagementApiPort, @IsActive);";
            return SqlDataAccess.ModifyDatabase(connString, sql, parameters);*/

            throw new NotImplementedException();
        }

        public static int RemoveServer(string serverIP, string connString)
        {
            //  Old way (not using stored procedures and using older function that took in a string sql)
            /*string sql = @"DELETE FROM dbo.ServerTable WHERE ServerIP=@ServerIP;";
            return SqlDataAccess.ModifyDatabase(sql, connString, new { ServerIP = serverIP });*/

            throw new NotImplementedException();
        }

        public static IEnumerable<HostModel> LoadServers(string connString)
        {
            //  Old way (not using stored procedures and using older function that took in a string sql)
            /*string sql = @"select * from dbo.ServerTable;";
            return SqlDataAccess.LoadData<HostModel>(sql, connString);*/

            throw new NotImplementedException();
        }
    }
}
