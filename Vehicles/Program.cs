using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vehicles
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            var query =
                from car in cars
                join manufacturer in manufacturers 
                    on car.Manufacturer equals manufacturer.Name
                //where car.Manufacturer == "BMW" && car.Year == 2016
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    manufacturer.Headquarters,
                    car.Name,
                    car.Combined
                };

            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
            };

            Console.WriteLine("========================================");

            var query2 =
                cars.Join(manufacturers,
                            c => c.Manufacturer,
                            m => m.Name,
                            (c, m) => new
                            {

                                m.Headquarters,
                                c.Name,
                                c.Combined
                            })
                      .OrderByDescending(c => c.Combined)
                      .ThenBy(c => c.Name);

            Console.WriteLine("========================================");
            var query3 =
                cars.Join(manufacturers,
                            c => c.Manufacturer,
                            m => m.Name,
                            (c, m) => new
                            {
                                Car = c,
                                Manufacturer = m
                            })
                      .OrderByDescending(c => c.Car.Combined)
                      .ThenBy(c => c.Car.Name)
                      .Select(c => new
                      {
                          c.Manufacturer.Headquarters,
                          c.Car.Name,
                          c.Car.Combined
                      });

            Console.WriteLine("========================================");
            var query4 =
                    from car in cars
                    join manufacturer in manufacturers
                        on  new { car.Manufacturer, car.Year } 
                            equals 
                            new { Manufacturer = manufacturer.Name, manufacturer.Year }
                    //where car.Manufacturer == "BMW" && car.Year == 2016
                    orderby car.Combined descending, car.Name ascending
                    select new
                    {
                        manufacturer.Headquarters,
                        car.Name,
                        car.Combined
                    };
            Console.WriteLine("========================================");

            var query5 =
                cars.Join(manufacturers,
                            c => new { c.Manufacturer, c.Year },
                            m => new { Manufacturer = m.Name, m.Year },
                            (c, m) => new
                            {

                                m.Headquarters,
                                c.Name,
                                c.Combined
                            })
                      .OrderByDescending(c => c.Combined)
                      .ThenBy(c => c.Name);
            Console.WriteLine("========================================");
            var query6 =
                    from car in cars
                    group car by car.Manufacturer;
            foreach (var result in query6)
            {
                //Console.WriteLine(result.Key);
                Console.WriteLine($"{result.Key} has {result.Count()} cars.");
            }
            Console.WriteLine("========================================");
            var query7 =
                    from car in cars
                    group car by car.Manufacturer.ToUpper();

            foreach (var group in query7)
            {
                Console.WriteLine($"{group.Key} has {group.Count()} cars.");
                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            Console.WriteLine("========================================");
            var query8 =
                    from car in cars
                    group car by car.Manufacturer.ToUpper() into manufacturer
                    orderby manufacturer.Key
                    select manufacturer;

            Console.WriteLine("========================================");
            var query9 =
                    cars.GroupBy(c => c.Manufacturer.ToUpper())
                        .OrderBy(g => g.Key);


            Console.WriteLine("========================================");
            var query10 =
                     from manufacturer in manufacturers
                     join car in cars on manufacturer.Name equals car.Manufacturer
                     into carGroup
                     select new
                     {
                         Manufacturer = manufacturer,
                         Cars = carGroup
                     };
            
            foreach (var group in query10)
            {
                Console.WriteLine($"{group.Manufacturer.Name}: {group.Manufacturer.Headquarters}");
                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            Console.WriteLine("========================================");
            var query11 =
                    manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
                        (m, g) =>
                        new 
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                    .OrderBy(m => m.Manufacturer.Headquarters);



            Console.WriteLine("========================================");
            var query12 =
                    from manufacturer in manufacturers
                    join car in cars on manufacturer.Name equals car.Manufacturer
                        into carGroup
                    select new
                    {
                        Manufacturer = manufacturer,
                        Cars = carGroup
                    } into result
                    group result by result.Manufacturer.Headquarters;

            foreach (var group in query12)
            {
                Console.WriteLine($"{group.Key}");
                foreach (var car in group.SelectMany(g => g.Cars)
                                        .OrderByDescending(c => c.Combined)
                                        .Take(3))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
            Console.WriteLine("========================================");
            var query13 =
                from car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Avg = carGroup.Average(c => c.Combined)
                };
            var query14 =
                from car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Avg = carGroup.Average(c => c.Combined)
                } into result
                orderby result.Max descending
                select result;

            foreach (var result in query13)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\t Max: {result.Max}");
                Console.WriteLine($"\t Min: {result.Min}");
                Console.WriteLine($"\t Avg: {result.Avg}");
            }
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query =
                    File.ReadAllLines(path)
                        .Where(l => l.Length > 1)
                        .Select(l =>
                        {
                            var columns = l.Split(',');
                            return new Manufacturer
                            {
                                Name = columns[0],
                                Headquarters = columns[1],
                                Year = int.Parse(columns[2])
                            };
                        });
            return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
                   File.ReadAllLines(path)
                        .Skip(1)
                        .Where(l => l.Length > 1)
                        .ToCar();
            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car
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
            }
        }
    }
}
