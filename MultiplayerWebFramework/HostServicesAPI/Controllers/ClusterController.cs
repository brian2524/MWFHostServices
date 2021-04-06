using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
/*        private readonly IHttpClientFactory _clientFactory;
        public ClusterController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }*/
        [HttpPost]
        public GameInstanceModel SpinUp([FromBody] JsonElement req)
        {
            // This was a system I was working on so that the endpoint can accept the "Game" parameter as an int or string and it will just parse what you sent
            /*Game reqGameCasted = Game.Game0;
            if (req.GetProperty("Game").TryGetInt32(out int reqGameInt))
            {
                reqGameCasted = (Game)reqGameInt;
            }
            else
            {
                string reqGameString = req.GetProperty("Game").GetString();
                if (Enum.TryParse(typeof(Game), reqGameString, out object game))
                {
                    reqGameCasted = (Game)game;
                }

            }*/

            Game reqGameCasted = (Game)(req.GetProperty("Game").GetInt32());
            string reqArgs     = req.GetProperty("Args").GetString();
            // Request will only give us the game to start and the arguments when starting it. Everything else will be decided by the Host (us)

            // This is BAD! this is only for testing (maybe add a cluster as a singleton in ConfigureServices instead)
            Cluster cluster = new Cluster();
            GameInstanceModel newGameInstance = cluster.SpinUp(reqGameCasted, reqArgs);

            if (newGameInstance == null)
            {
                return null;
            }
            // create game instance on the DB, set the new Id of the newGameInstance, and return the model
            return newGameInstance;
        }
    }
}
