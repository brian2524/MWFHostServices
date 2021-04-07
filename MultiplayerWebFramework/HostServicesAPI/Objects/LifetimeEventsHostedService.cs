using HostServicesAPI.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HostServicesAPI.Objects
{
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICluster _gameInstanceCluster;

        public LifetimeEventsHostedService(IHostApplicationLifetime appLifetime, IHttpClientFactory clientFactory, ICluster gameInstanceCluster)
        {
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

        private void OnStarted()
        {
            // Idea for how we should be adding the new host to the database
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/CreateHostAndReturnId", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
        }

        private void OnStopping()
        {
            // Here we should make sure to shut down all game instances and remove them from the database



            // After all game instaces from this host are shutdown and removed from the database, we must remove the host model from the database (must happen after since removing the host model before could result in a rejection since there may be forign keys from Game Instances referenceing it)
            // Also how do we want to handle cases where removing game instance from database fails? Should we not then remove the host from the database at all since this leaves a possibility the host won't be removed? Should we maybe add to the stored procedure for removing a host to remove all game instances that have forien keys to it? 
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/RemoveHostById", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
        }
    }
}

