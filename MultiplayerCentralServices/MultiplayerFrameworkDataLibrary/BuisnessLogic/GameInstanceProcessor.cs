using MultiplayerFrameworkDataLibrary.DataAccess;
using MultiplayerFrameworkModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerFrameworkDataLibrary.BuisnessLogic
{
    public static class GameInstanceProcessor
    {
        public static int CreateGameInstance(string game, string args, string associatedServer, string connString)
        {
            var data = new
            {
                Game = game,
                Args = args,
                AssociatedServer = associatedServer
            };

            string sql = @"insert into dbo.GameInstanceTable (Game, Args, AssociatedServer)
                         values (@Game, @Args, @AssociatedServer);";
            return SqlDataAccess.ModifyDatabase(sql, data, connString);
        }

        public static int RemoveGameInstance(int id, string connString)
        {
            string sql = @"DELETE FROM dbo.GameInstanceTable WHERE Id=@Id;";
            return SqlDataAccess.ModifyDatabase(sql, new { Id = id }, connString);
        }

        public static List<GameInstanceModel> LoadGameInstances(string connString)
        {
            string sql = @"select * from dbo.GameInstanceTable;";
            return SqlDataAccess.LoadData<GameInstanceModel>(sql, connString);
        }
    }
}
