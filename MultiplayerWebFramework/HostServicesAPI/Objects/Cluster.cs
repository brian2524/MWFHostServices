using HostServicesAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using MWFModelsLibrary.Enums;
using MWFModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using MWFDataLibrary.BuisnessLogic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace HostServicesAPI.Objects
{
    /**
    *      Pourpose of this object is to provide an easy/simple way to create/shutdown game instances while keeping it in sync
    *      with the database. If we can have a one-to-one match up of the actual instanced processes to the database entries at
    *      all times without having to think about it, we won't encounter any troubles or confustion and game instance management 
    *      becomes simple.
    */
    public class Cluster : ICluster
    {
        public List<GameInstanceModel> ActiveGameInstances { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;
        public Cluster(IHttpClientFactory httpClientFactory)
        {
            ActiveGameInstances = new List<GameInstanceModel>();
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HttpResponseMessage> SpinUpGameInstance(Game game, string port, string args, int hostId, string filePath)
        {
            Process newProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = filePath,
                    Arguments = args,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };
            if (port != "")
            {
                newProcess.StartInfo.Arguments += (" -port=" + port);
            }

            if (newProcess.Start() == true)
            {
                GameInstanceModel newGameInstanceModel = new GameInstanceModel { ProcessId = newProcess.Id };
                ActiveGameInstances.Add(newGameInstanceModel);

                HttpClient client = _httpClientFactory.CreateClient("MWFHostServicesAPIClient");
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(@"CreateGameInstanceAndReturnId", new { ProcessId = newGameInstanceModel.ProcessId, Game = (int)game, Port = port, Args = args, HostId = hostId }, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (responseMessage.IsSuccessStatusCode)
                {
                    // Finish setting new game instance model's fields now that we know it was added to the db
                    int id = await HttpContentJsonExtensions.ReadFromJsonAsync<int>(responseMessage.Content);
                    newGameInstanceModel.Id = id;
                    newGameInstanceModel.Game = game;
                    newGameInstanceModel.Port = port;
                    newGameInstanceModel.Args = args;
                    newGameInstanceModel.HostId = hostId;

                    return responseMessage;
                }
                else
                {
                    // If the process started up correctly but wasn't successfully added to the db....
                    ActiveGameInstances.Remove(newGameInstanceModel);
                    newProcess.Kill();
                    // Should probably call its IDisposable
                }
            }


            return new HttpResponseMessage(HttpStatusCode.Conflict);
        }

        public async Task<HttpResponseMessage> ShutDownGameInstanceById(int inId)
        {
            GameInstanceModel modelToRemove = null;
            foreach (GameInstanceModel item in ActiveGameInstances)
            {
                if (item?.Id == inId)
                {
                    modelToRemove = item;
                }
            }
            if (modelToRemove == null)
            {
                // We don't want to be able to modify the db if we aren't modifying locally
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            HttpClient client = _httpClientFactory.CreateClient("MWFHostServicesAPIClient");
            HttpResponseMessage responseMessage = await client.DeleteAsync(@"DeleteGameInstanceById/?Id=" + inId);
            if (responseMessage.IsSuccessStatusCode)
            {
                // Removed from db, so lets remove and shut it down locally
                ActiveGameInstances.Remove(modelToRemove);
                Process.GetProcessById(modelToRemove.ProcessId).Kill();
                // Should probably call its IDisposable

                return responseMessage;
            }



            return new HttpResponseMessage(HttpStatusCode.Conflict);
        }

        public async Task<bool> ShutDownAllGameInstances(int hostId)
        {
            HttpClient client = _httpClientFactory.CreateClient("MWFHostServicesAPIClient");
            HttpResponseMessage responseMessage = await client.DeleteAsync(@"DeleteGameInstancesByHostId/?HostId=" + hostId);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                int dbGameInstancesDeleted = int.Parse(responseBody);
                if (dbGameInstancesDeleted == ActiveGameInstances.Count)
                {

                }
            }
            foreach (GameInstanceModel item in ActiveGameInstances)
            {
                Process procToKill = Process.GetProcessById(item.ProcessId);

                procToKill?.Kill();
                // Should probably call its IDisposable
            }
            ActiveGameInstances.Clear();

            return true;
        }
    }
}