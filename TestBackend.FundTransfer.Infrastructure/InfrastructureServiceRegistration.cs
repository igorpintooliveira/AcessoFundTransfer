using Microsoft.Extensions.DependencyInjection;
using TestBackend.FundTransfer.Application.Contracts.Infrastructure;

namespace TestBackend.FundTransfer.Infrastructure.Redis
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureRedisServices(this IServiceCollection services)
        {
            services.AddScoped<IFundTransferInQueue, FundTransferInQueue>();

            return services;
        }
    }
}
