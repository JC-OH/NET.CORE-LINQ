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
                    Console.WriteLine($"{car.Name}");
                }
            }
        }
    }
}
