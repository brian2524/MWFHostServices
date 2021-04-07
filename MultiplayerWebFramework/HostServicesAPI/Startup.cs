using HostServicesAPI.Interfaces;
using HostServicesAPI.Objects;
using Microsoft.AspNetCore.Authentication.Certificate;
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
            //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();    // Add this service to avoid net::ERR_CERT_AUTHORITY_INVALID error when calling on this web api

            services.AddCors();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HostServicesAPI", Version = "v1" });
            });





            services.AddHttpClient("MWFHostServicesAPIClient", client =>                                            // Add HttpClientFactory
            {
                client.BaseAddress = new Uri(@"http://localhost:7071/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));    // Just give us json (we are not looking for a web page or anything)
            });

            // ------------------------------------------------------------
            // This is how you can make sure only one singleton instance of a concrete class will be made when there are multiple interfaces being implemented for it
            services.AddSingleton<SetupTeardownHostedService>();    // First create the single instance
            // now we need to fill in its dependencies............
            services.AddSingleton<IHostedService>(x => x.GetRequiredService<SetupTeardownHostedService>());         // Forward requests to our concrete class
            services.AddSingleton<IMWFHostModel>(x => x.GetRequiredService<SetupTeardownHostedService>());  // Forward requests to our concrete class
            // ------------------------------------------------------------

            services.AddSingleton<ICluster, Cluster>();                                                             // Add our cluster as an app singleton
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            //app.UseAuthentication();        // This will set the HttpContext.User to be set to ClaimsPrincipal avoids net::ERR_CERT_AUTHORITY_INVALID error when calling on this web api


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HostServicesAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
