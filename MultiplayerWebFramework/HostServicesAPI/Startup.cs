using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using MWFModelsLibrary.Models;

namespace HostServicesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Idea for how we should be adding the new host to the database
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/CreateHostAndReturnId", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            hostApplicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        private void OnShutdown()
        {
            // Here we should make sure to shut down all game instances and remove them from the database



            // After all game instaces from this host are shutdown and removed from the database, we must remove the host model from the database (must happen after since removing the host model before could result in a rejection since there may be forign keys from Game Instances referenceing it)
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/RemoveHostById", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
        }
    }
}
