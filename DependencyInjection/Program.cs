using DependencyInjection.Abstractions;
using DependencyInjection.Implementation;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace DependencyInjection
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            ResigerServices();

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run();
            }
            
            DisposeServies();
        }

        private static void ResigerServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICustomer, Customer>();
            services.AddSingleton<ConsoleApplication>();

            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServies()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

    }
}
