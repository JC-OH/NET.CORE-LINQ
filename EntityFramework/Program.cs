using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {

            Func<int, int> square = x => x * x;
            Expression<Func<int, int, int>> add = (x, y) => x + y;
            Func<int, int, int> addI = add.Compile();

            var result = addI(3, 5);
            Console.WriteLine(add);
            Console.WriteLine(result);

            InsertData();
            QueryData();
        }

        private static void InsertData()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Skip(1)
                    .Select(l => {
                        var columns = l.Split(',');
                        return new Car
                        {
                            Year = int.Parse(columns[0]),
                            Manufacturer = columns[1],
                            Name = columns[2],
                            Displacement = double.Parse(columns[3]),
                            Cylinders = int.Parse(columns[4]),
                            City = int.Parse(columns[5]),
                            Highway = int.Parse(columns[6]),
                            Combined = int.Parse(columns[7])
                        };
                    });
            return query.ToList();
        }

        private static void QueryData()
        {
            var db = new CarDb();
            db.Database.Log = Console.WriteLine;

            var query = from car in db.Cars
                        orderby car.Combined descending, car.Name ascending
                        select car;

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Name}: {car.Combined}");
            }
            Console.WriteLine("======================================================");
            var query1 =
                db.Cars.OrderByDescending(c => c.Combined).ThenBy(c => c.Name).Take(10);

            foreach (var car in query1)
            {
                Console.WriteLine($"{car.Name}: {car.Combined}");
            }
            Console.WriteLine("======================================================");
            var query2 =
                db.Cars.Where(c => c.Manufacturer == "BMW")
                        .OrderByDescending(c => c.Combined)
                        .ThenBy(c => c.Name)
                        .Take(10);
            foreach (var car in query2)
            {
                Console.WriteLine($"{car.Name}: {car.Combined}");
            }
            Console.WriteLine("======================================================");

            var query3 =
                db.Cars.Where(c => c.Manufacturer == "BMW")
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Take(10)
                    .ToList();
            Console.WriteLine(query3.Count());


            /*var query4 =
                db.Cars.Where(c => c.Manufacturer == "BMW")
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Take(10)
                    .Select(c => new { Name = c.Name.Split(' ') })
                    .ToList();*/
            Console.WriteLine("======================================================");

            var query5 =
                db.Cars.GroupBy(c => c.Manufacturer)
                    .Select(g => new
                    {
                        Name = g.Key,
                        Cars = g.OrderByDescending(c => c.Combined).Take(2)
                    });

            var query6 =
                    from car in db.Cars
                    group car by car.Manufacturer into manfacturer
                    select new
                    {
                        Name = manfacturer.Key,
                        Cars = (from car in manfacturer
                                orderby car.Combined descending
                                select car).Take(2)
                    };
            
            foreach (var group in query5)
            {
                Console.WriteLine(group.Name);
                foreach(var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name}: {car.Combined}");
                }
            }



        }

    }
}
