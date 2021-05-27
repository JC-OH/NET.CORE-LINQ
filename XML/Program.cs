using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XML
{
    class Program
    {
        static void Main(string[] args)
        {
            var records = ProcessCars("fuel.csv");
            //Console.WriteLine(records.GetType());

            var document = new XDocument();
            var cars = new XElement("Cars");

            foreach (var record in records)
            {
                var car = new XElement("Car");
                var name = new XElement("Name", record.Name);
                var combined = new XElement("Combined", record.Combined);
                car.Add(name);
                car.Add(combined);
                cars.Add(car);
            }

            document.Add(cars);
            document.Save("fuel1.xml");
            //==================================================================
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Skip(1)
                    .Select(l => {
                        var columns = l.Split(",");

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
    }
}
