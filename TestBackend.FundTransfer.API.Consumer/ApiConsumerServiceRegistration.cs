using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.API.Consumer
{
    public static class ApiConsumerServiceRegistration
    {
        public static IServiceCollection AddApiConsumerServices(this IServiceCollection services)
        {            

            return services;
        }
    }
}
