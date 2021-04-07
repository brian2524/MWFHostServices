﻿using HostServicesAPI.Interfaces;
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
    public class Cluster : ICluster
    {
        public List<GameInstanceModel> GameInstances { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;
        public Cluster(IHttpClientFactory httpClientFactory)
        {
            GameInstances = new List<GameInstanceModel>();
            _httpClientFactory = httpClientFactory;
        }
        public async Task<HttpResponseMessage> SpinUp(Game game, string port, string args, string filePath)
        {
            int hostId = 3;
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
                HttpClient client = _httpClientFactory.CreateClient("MWFHostServicesAPIClient");
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(@"http://localhost:7071/api/CreateGameInstanceAndReturnId", new { Game = (int)game, Port = port, Args = args, HostId = hostId }, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (responseMessage.IsSuccessStatusCode)
                {
                    int id = await HttpContentJsonExtensions.ReadFromJsonAsync<int>(responseMessage.Content);
                    // Still need to fill in correct host id but this is a good start for now
                    GameInstances.Add(new GameInstanceModel
                    {
                        Id = id,
                        Game = game,
                        Port = port,
                        Args = args,
                        HostId = hostId     // Not accurate yet. Need to implement this application adding itself to db and getting it's ID so we know this
                    });
                    return responseMessage;
                }
                else
                {
                    // If the process started up correctly but wasn't successfully added to the db....
                    newProcess.Kill();
                }
            }
            

            return new HttpResponseMessage(HttpStatusCode.Conflict);
        }

        private static string GetMachineIP()
        {
            string localIp;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIp = endPoint.Address.ToString();
            }

            return localIp;
        }
    }
}
