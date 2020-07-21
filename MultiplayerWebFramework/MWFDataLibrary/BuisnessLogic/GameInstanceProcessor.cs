using MWFDataLibrary.DataAccess;
using MultiplayerFrameworkModelsLibrary.Models;
using System;
using System.Collections.Generic;

namespace MWFDataLibrary.BuisnessLogic
{
    public static class GameInstanceProcessor
    {
        public static int CreateGameInstance(string game, string args, string associatedServer, string connString)
        {
            var parameters = new
            {
                Game = game,
                Args = args,
                AssociatedServer = associatedServer
            };

            //  Old way (not using stored procedures and using older function that took in a string sql)
            /*string sql = @"insert into dbo.GameInstanceTable (Game, Args, AssociatedServer)
                         values (@Game, @Args, @AssociatedServer);";
            return SqlDataAccess.ModifyDatabase(connString, sql, parameters);*/

            /*SaveData(connection string here, "spGameInstance_CreateAndOutputId", object*//*T*//* parameters)*/

            throw new NotImplementedException();
        }

        public static int RemoveGameInstance(int id, string connString)
        {
            //  Old way (not using stored procedures and using older function that took in a string sql)
            /*string sql = @"DELETE FROM dbo.GameInstanceTable WHERE Id=@Id;";
            return SqlDataAccess.ModifyDatabase(sql, connString, new { Id = id });*/

            throw new NotImplementedException();
        }

        public static IEnumerable<GameInstanceModel> LoadGameInstances(string connString)
        {
            //  Old way (not using stored procedures and using older function that took in a string sql)
            /*string sql = @"select * from dbo.GameInstanceTable;";
            return SqlDataAccess.LoadData<GameInstanceModel>(sql, connString);*/

            throw new NotImplementedException();
        }
    }
}
