using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EntityFrameworkCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new CarDb())
            {
                var query =
                    db.Cars.Where(c => c.Manufacturer == "BMW")
                            .Select(c => new
                            {
                                Name = c.Name
                            });
                foreach (var car in query.ToList())
                {
                    //Console.WriteLine($"{car.Name}");
                }
            }

            Console.WriteLine("============================================");
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            Console.WriteLine(configuration["Message"]);
            var testOPT = configuration.GetSection("TestOPT");
            Console.WriteLine(testOPT["id"]);

            string connectionString = configuration.GetValue<string>("ServiceBusConnectionString");

            Console.WriteLine(connectionString);

            Console.WriteLine("============================================");
            var loggingConfig = new LoggingConfig();
            configuration.Bind("Logging", loggingConfig);

            Console.WriteLine(loggingConfig.LogPath);
            Console.WriteLine(loggingConfig.Level);

            Console.WriteLine("============================================");

            /*IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", true, true)
                            .AddUserSecrets<Program>()
                            .Build();*/

        }
    }
}
