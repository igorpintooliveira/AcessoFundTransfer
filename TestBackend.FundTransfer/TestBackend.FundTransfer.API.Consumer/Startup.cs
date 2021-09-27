using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBackend.FundTransfer.API.Consumer.EventBusConsumer;
using TestBackend.FundTransfer.Application;
using TestBackend.FundTransfer.EventBus.Commom;
using TestBackend.FundTransfer.Infrastructure.Mongo;
using TestBackend.FundTransfer.Infrastructure.Mongo.Data;
using TestBackend.FundTransfer.Infrastructure.Mongo.Data.Interfaces;
using TestBackend.FundTransfer.Infrastructure.Redis;

namespace TestBackend.FundTransfer.API.Consumer
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
            // Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("RedisCacheSettings:ConnectionString");
            });

            // Services Registration
            services.AddApiConsumerServices();
            services.AddApplicationServices(Configuration);
            services.AddInfrastructureRedisServices();
            services.AddInfrastructureMongoServices();

            // MassTransit/RabbitMQ Configuration
            services.AddMassTransit(config => {

                config.AddConsumer<FundTransferConsumer>();

                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(Configuration["RabbitEventBusSettings:HostAddress"]);

                    cfg.ReceiveEndpoint(EventBusConstants.FundTransferQueue, c => {
                        c.ConfigureConsumer<FundTransferConsumer>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();

            // General Configuration
            services.AddScoped<FundTransferConsumer>();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FundTransfer.API.Consumer", Version = "v1" });
            });

            services.AddHealthChecks()
                    .AddRedis(Configuration["RedisCacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);

            services.AddHealthChecks()
                    .AddMongoDb(Configuration["MongoDatabaseSettings:ConnectionString"], "Mongo Health", HealthStatus.Degraded);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FundTransfer.API.Consumer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
