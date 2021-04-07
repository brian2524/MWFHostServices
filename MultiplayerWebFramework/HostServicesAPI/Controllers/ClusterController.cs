using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HostServicesAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MWFModelsLibrary.Enums;
using MWFModelsLibrary.Models;

namespace HostServicesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClusterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICluster _gameInstanceCluster;
        public ClusterController(IConfiguration Configuration, ICluster gameInstanceCluster, IHttpClientFactory clientFactory)
        {
            _configuration = Configuration;
            _clientFactory = clientFactory;
            _gameInstanceCluster = gameInstanceCluster;
        }
        [HttpPost]
        public async Task<IActionResult> SpinUp([FromBody] JsonElement req)
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

            Game reqGameCasted;
            string reqPort;
            string reqArgs;
            try
            {
                reqGameCasted = (Game)(req.GetProperty("Game").GetInt32());
                reqPort = req.GetProperty("Port").GetString();
                reqArgs = req.GetProperty("Args").GetString();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Request didn't meet syntax requirements (make sure you include everything and have the correct property types)");
            }





            HttpResponseMessage spinUpResponseMessage = null;
            switch (reqGameCasted)
            {
                case Game.Game0:
                    {
                        spinUpResponseMessage = await _gameInstanceCluster.SpinUp(reqGameCasted, reqPort, reqArgs, _configuration.GetValue<string>("GameFilePaths:ALSReplicated"));
                    }
                    break;
                case Game.Game1:
                    break;
            }


            if (spinUpResponseMessage == null)
            {
                return new BadRequestObjectResult("Passed in game doesn't exist on this host");
            }







            // Lets work on creating an ObjectResult based off of the cluster's spinup response
            ObjectResult retObjResult = StatusCode((int)(spinUpResponseMessage.StatusCode), spinUpResponseMessage.Content);
            retObjResult.Value = "Value of the object result (type object and ends up in the body)";
            return retObjResult;
        }
    }
}
