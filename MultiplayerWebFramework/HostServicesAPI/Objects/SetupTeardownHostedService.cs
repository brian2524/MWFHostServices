using HostServicesAPI.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MWFModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HostServicesAPI.Objects
{
    internal class SetupTeardownHostedService : IHostedService, IApplicationHostModel
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICluster _gameInstanceCluster;

        // The Host model from the database that identifies this HostServicesApi application
        public HostModel applicationHostModel { get; private set; }

        public SetupTeardownHostedService(ILogger<SetupTeardownHostedService> logger, IHostApplicationLifetime appLifetime, IHttpClientFactory clientFactory, ICluster gameInstanceCluster)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _clientFactory = clientFactory;
            _gameInstanceCluster = gameInstanceCluster;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        // We can return void asynchronously since this is a callback
        private async void OnStarted()
        {
            applicationHostModel = new HostModel { HostIp = GetMachineIP(), HostServicesAPISocketAddress = GetMachineIP(), IsActive = true };

            HttpResponseMessage httpResponse = null;
            try
            {
                _logger.Log(LogLevel.Information, "Attempting to register us with the database (creating db host entry)");
                HttpClient client = _clientFactory.CreateClient("MWFHostServicesAPIClient");
                httpResponse = await client.PostAsJsonAsync<HostModel>(@"http://localhost:7071/api/CreateHostAndReturnId", applicationHostModel, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical, e, "We must shut down since we can't be added to the database");
                _appLifetime.StopApplication();
            }
            finally
            {
                if (httpResponse?.IsSuccessStatusCode == false)
                {
                    _logger.Log(LogLevel.Critical, "Unsuccessful status code: " + httpResponse.StatusCode.ToString() + "\nWe must shut down since we can't be added to the database");
                    _appLifetime.StopApplication();
                }
                else if (httpResponse?.IsSuccessStatusCode == true)
                {
                    _logger.Log(LogLevel.Information, "Successful status code: " + httpResponse.StatusCode.ToString() + "\nHost added to database! API is now ready for requests!");
                    int id = await HttpContentJsonExtensions.ReadFromJsonAsync<int>(httpResponse.Content);
                    applicationHostModel.Id = id;
                }
            }

            
            
        }

        // We can return void asynchronously since this is a callback
        private void OnStopping()
        {
            // Here we should make sure to shut down all game instances and remove them from the database



            // After all game instaces from this host are shutdown and removed from the database, we must remove the host model from the database (must happen after since removing the host model before could result in a rejection since there may be forign keys from Game Instances referenceing it)
            // Also how do we want to handle cases where removing game instance from database fails? Should we not then remove the host from the database at all since this leaves a possibility the host won't be removed? Should we maybe add to the stored procedure for removing a host to remove all game instances that have forien keys to it? 
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/RemoveHostById", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
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

