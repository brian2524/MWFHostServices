using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClusterHandlerLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MWFModelsLibrary.Enums;
using MWFModelsLibrary.Models;

namespace HostServicesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClusterController : ControllerBase
    {
        [HttpPost]
        public GameInstanceModel SpinUp([FromBody] JsonElement spinUpData)
        {
            Game game = (Game)(spinUpData.GetProperty("Game").GetInt32());
            string args = spinUpData.GetProperty("Args").GetString();

            // This is BAD! this is only for testing (maybe add a cluster as a singleton in ConfigureServices instead)
            Cluster cluster = new Cluster();
            GameInstanceModel newGameInstance = cluster.SpinUp(game, args);

            if (newGameInstance == null)
            {
                return null;
            }
            // create game instance on the DB, set the new Id of the newGameInstance, and return the model
            return newGameInstance;//
        }
    }
}
