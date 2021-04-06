using HostServicesAPI.Interfaces;
using HostServicesAPI.Objects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HostServicesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HostServicesAPI", Version = "v1" });
            });


            services.AddHttpClient("MWFHostServicesAPIClient", client =>
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));    // Give us json
            });

            services.AddSingleton<ICluster, Cluster>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HostServicesAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            hostApplicationLifetime.ApplicationStarted.Register(OnApplicationStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnShutdown);   // Bind to shutdown signal
        }

        private void OnApplicationStarted()
        {
            // Idea for how we should be adding the new host to the database
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/CreateHostAndReturnId", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
        }

        private void OnShutdown()
        {
            // Here we should make sure to shut down all game instances and remove them from the database



            // After all game instaces from this host are shutdown and removed from the database, we must remove the host model from the database (must happen after since removing the host model before could result in a rejection since there may be forign keys from Game Instances referenceing it)
            // Also how do we want to handle cases where removing game instance from database fails? Should we not then remove the host from the database at all since this leaves a possibility the host won't be removed? Should we maybe add to the stored procedure for removing a host to remove all game instances that have forien keys to it? 
            /*var result = Http.PostAsJson<GameInstanceModel>(@"http://localhost:7071/api/RemoveHostById", newGameInstanceToAdd, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });*/
        }
    }
}
