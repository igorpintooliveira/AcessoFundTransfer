using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestBackend.FundTransfer.Application.Contracts.Infrastructure;
using TestBackend.FundTransfer.Infrastructure.Mongo.Data;
using TestBackend.FundTransfer.Infrastructure.Mongo.Data.Interfaces;
using TestBackend.FundTransfer.Infrastructure.Mongo.Repositories;

namespace TestBackend.FundTransfer.Infrastructure.Mongo
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureMongoServices(this IServiceCollection services)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IFundTransferContext, FundTransferContext>();
            services.AddScoped<IFundTransferRepository, FundTransferRepository>();

            return services;
        }
    }
}
