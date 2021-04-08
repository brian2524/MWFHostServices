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
    internal class SetupTeardownHostedService : IHostedService, IMWFHostModel
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICluster _gameInstanceCluster;

        public HostModel applicationHostModel { get; set; }

        // Injected into ClusterController since injecting into the cluster class causes circular dependency in ICluster service.
        public SetupTeardownHostedService(ILogger<SetupTeardownHostedService> logger, IHostApplicationLifetime appLifetime, IHttpClientFactory clientFactory, ICluster gameInstanceCluster)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _clientFactory = clientFactory;
            _gameInstanceCluster = gameInstanceCluster;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            applicationHostModel = new HostModel { HostIp = GetMachineIP(), HostServicesAPISocketAddress = GetMachineIP(), IsActive = true };
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
            string machineIp = GetMachineIP();
            applicationHostModel = new HostModel 
            {
                HostIp = machineIp,
                HostServicesAPISocketAddress = machineIp,
                IsActive = true 
            };

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
                return;
            }
               
            if (httpResponse?.IsSuccessStatusCode == false)
            {
                _logger.Log(LogLevel.Critical, "Unsuccessful status code: " + httpResponse.StatusCode.ToString() + "\nWe must shut down since we can't be added to the database");
                _appLifetime.StopApplication();
                return;
            }
            else if (httpResponse?.IsSuccessStatusCode == true)
            {
                _logger.Log(LogLevel.Information, "Successful status code: " + httpResponse.StatusCode.ToString() + "\nHost added to database. HostServicesAPI is now ready for requests!");
                int id = await HttpContentJsonExtensions.ReadFromJsonAsync<int>(httpResponse.Content);
                applicationHostModel.Id = id;
            }

            
            
        }

        // We can return void asynchronously since this is a callback
        private async void OnStopping()
        {
            if(await _gameInstanceCluster.ShutDownAllGameInstances(applicationHostModel.Id))
            {
                _logger.Log(LogLevel.Information, "Successfully removed all game instances locally and from db");
            }


            HttpClient client = _clientFactory.CreateClient("MWFHostServicesAPIClient");
            HttpResponseMessage responseMessage = await client.DeleteAsync(@"http://localhost:7071/api/DeleteHostById/?Id=" + applicationHostModel.Id);
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

