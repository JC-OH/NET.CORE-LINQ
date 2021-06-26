using DependencyInjection.Abstractions;
using DependencyInjection.Implementation;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Reflection;

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
            Console.WriteLine("================================");
            services.AddSingleton<ICustomer, Customer>();
            services.AddSingleton<ConsoleApplication>();
            
            Console.WriteLine("================================");
            Assembly ConsoleAppAssembly = typeof(Program).Assembly;

            var ConsoleAppTypes =
                from type in ConsoleAppAssembly.GetTypes()
                where !type.IsAbstract
                where typeof(ICustomer).IsAssignableFrom(type)
                select type;
            foreach (var type in ConsoleAppTypes)
            {
                Console.WriteLine(type.FullName);
                services.AddTransient(typeof(ICustomer), type);
            }
            Console.WriteLine(ConsoleAppTypes.Count());

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
