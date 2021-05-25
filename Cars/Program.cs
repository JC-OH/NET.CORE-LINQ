using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var query = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);

            foreach (var car in query.Take(10))
            {
                //Console.WriteLine(car.Name);
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }

            Console.WriteLine("=======================================");
            var query1 = from car in cars
                         where car.Manufacturer == "BMW" && car.Year == 2016
                         orderby car.Combined descending, car.Name ascending
                         select car;

            var query2 =
                    cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Select(c => c)
                    .First();

            var query3 =
                    cars
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Select(c => c)
                    .First(c => c.Manufacturer == "BMW" && c.Year == 2016);

            var query4 = (from car in cars
                         where car.Manufacturer == "BMW" && car.Year == 2016
                         orderby car.Combined descending, car.Name ascending
                         select car).First();

            var query5 =
                    cars
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Select(c => c)
                    .Last(c => c.Manufacturer == "BMW" && c.Year == 2016);

            var top =
                    cars
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Select(c => c)
                    .FirstOrDefault();
            if (top != null)
            {
                Console.WriteLine(top.Name);
            }

            foreach (var car in query1.Take(10))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");            
            }

            Console.WriteLine("=======================================");
            foreach (var car in query1.Take(10).Reverse())
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }
            Console.WriteLine("=======================================");
            var result = cars.Any();

            var result1 = cars.Any(c => c.Manufacturer == "Ford");
            var result2 = cars.All(c => c.Manufacturer == "Ford");
            Console.WriteLine("=======================================");
            var anno = from car in cars
                         where car.Manufacturer == "BMW" && car.Year == 2016
                         orderby car.Combined descending, car.Name ascending
                         select new { 
                            car.Manufacturer,
                            car.Name,
                            car.Combined
                         };

            Console.WriteLine("=======================================");

            var result3 = anno.Select(c => c.Name);

            foreach (var name in result3)
            {
                foreach (var character in name)
                {
                    Console.WriteLine(character);
                }
            }

            Console.WriteLine("=======================================");
            var result4 = anno.SelectMany(c => c.Name);

            foreach (var character in result4)
            {
                Console.WriteLine(character);
            }

        }

        private static List<Car> ProcessFile(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .Select(Car.ParseFromCsv);

            var query1 =
                from line in File.ReadAllLines(path).Skip(1)
                where line.Length > 1
                select Car.ParseFromCsv(line);

            var query2 =

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
