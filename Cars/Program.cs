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
                         orderby car.Combined descending, car.Name ascending
                         select car;
            foreach (var car in query1.Take(10))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");            
            }

            Console.WriteLine("=======================================");
            foreach (var car in query1.Take(10).Reverse())
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
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

            return query.ToList();
        }
    }
}
