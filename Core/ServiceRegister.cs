using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceRegister
    {
        public static IServiceCollection Register(IServiceCollection service)
        {
      
            service.AddTransient<OrderService>();

            service.AddMemoryCache();
            return service;
        }
    }
}