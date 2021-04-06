using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HostServicesAPI.Interfaces;
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
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICluster _gameInstanceCluster;
        public ClusterController(ICluster gameInstanceCluster, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _gameInstanceCluster = gameInstanceCluster;
        }
        [HttpPost]
        public async Task SpinUp([FromBody] JsonElement req)
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
            string reqPort     = req.GetProperty("Port").GetString();
            string reqArgs     = req.GetProperty("Args").GetString();
            // Request will only give us the game to start, the port, and the arguments when starting it. Everything else will be decided by the Host (us)

            GameInstanceModel newGameInstance = await _gameInstanceCluster.SpinUp(reqGameCasted, reqPort, reqArgs);
        }
    }
}
