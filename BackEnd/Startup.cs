using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using BackEnd.Controllers;
using BackEnd.Config;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using Microsoft.AspNetCore.Http;
using BackEnd.Services.Implementations;

namespace BackEnd
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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddControllers();

            //register services
            services.AddSingleton<IBackEnd , BackEndSvc>(); 

            //swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AGL Cats API assessment",
                    Version = "v1",
                    Description = "HTTP API"
                });
               // options.ExampleFilters();
            });
 
           
            services.Configure<ConfigSettings>(Configuration.GetSection("Settings"));

       

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim("rol", "api-access"));
            });


            services.AddApplicationServices();

            services.AddAutoMapper(typeof(Startup));
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("AllowAll");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
            //swagger
            var vd = Configuration["VirtualDirectory"];
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(vd) ? vd : string.Empty) }/swagger/v1/swagger.json", "TEST.API V1");
                   c.InjectStylesheet("/swagger-ui/custom.css");
               });


        }

       
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            try
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                //register http services 
                //commented as handling by injection for testing 
                //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                // .AddPolicyHandler(GetRetryPolicy());
                // .AddPolicyHandler(GetCircuitBreakerPolicy()); 

                 services.AddHttpClient<ICats, Cats>();


            }
            catch (Exception e) { throw new Exception("Error1 " + e.Message, e); }

            return services;
        }


        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound || msg.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
              .Or<Polly.Timeout.TimeoutRejectedException>()
              .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));
        }

        static IAsyncPolicy<HttpResponseMessage> Timeout(int seconds = 25) =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
    }
}
