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

            //==================================================================
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
            document = new XDocument();
            cars = new XElement("Cars");
            foreach (var record in records)
            {
                /*var car = new XElement("Car");
                var name = new XAttribute("Name", record.Name);
                var combined = new XAttribute("Combined", record.Combined);
                car.Add(name);
                car.Add(combined);*/

                /*var name = new XAttribute("Name", record.Name);
                var combined = new XAttribute("Combined", record.Combined);
                var car = new XElement("Car", name, combined);*/

                var car = new XElement("Car",
                                        new XAttribute("Name", record.Name),
                                        new XAttribute("Combined", record.Combined),
                                        new XAttribute("Manufacturer", record.Manufacturer)
                                       );

                cars.Add(car);
            }
            document.Add(cars);
            document.Save("fuel2.xml");
            //==================================================================
            Console.WriteLine("[CreateXml]================================================");
            CreateXml(records);
            Console.WriteLine("[QueryXml]================================================");
            QueryXml();
            Console.WriteLine("[CreateXmlWithNameSpace]================================================");
            //==================================================================
            CreateXmlWithNameSpace(records);
            //==================================================================
            Console.WriteLine("[QueryXmlWithNameSpace]================================================");
            QueryXmlWithNameSpace();
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("fuel3.xml");

            var query =
                    from element in document.Element("Cars").Elements("Car")
                    //where element.Attribute("Manufacturer").Value == "BMW"
                    where element.Attribute("Manufacturer")?.Value == "BMW"
                    select element.Attribute("Name").Value;

            var query1 =
                    from element in document.Descendants("Car")
                    where element.Attribute("Manufacturer")?.Value == "BMW"
                    select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml(List<Car> records)
        {
            var document = new XDocument();
            /*cars = new XElement("Cars");
            var elements =
                    from record in records
                    select new XElement("Car",
                                        new XAttribute("Name", record.Name),
                                        new XAttribute("Combined", record.Combined),
                                        new XAttribute("Manufacterer", record.Manufacturer)
                                       );
            cars.Add(elements);*/

            var cars = new XElement("Cars",
                from record in records
                select new XElement("Car",
                                    new XAttribute("Name", record.Name),
                                    new XAttribute("Combined", record.Combined),
                                    new XAttribute("Manufacturer", record.Manufacturer)
                                    ));
            document.Add(cars);
            document.Save("fuel3.xml");
        }
        private static void CreateXmlWithNameSpace(List<Car> records)
        {
            var document = new XDocument();
            
            //XNamespace ns = "http://pluralsight.com/cars/20216";
            var ns = (XNamespace)"http://pluralsight.com/cars/20216";
            var ex = (XNamespace)"http://pluralsight.com/cars/20216/ex";

            var cars = new XElement(ns + "Cars",
                from record in records
                select new XElement(ex + "Car",
                                    new XAttribute("Name", record.Name),
                                    new XAttribute("Combined", record.Combined),
                                    new XAttribute("Manufacturer", record.Manufacturer)
                                    ));
            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            document.Add(cars);
            document.Save("fuel4.xml");
        }
        private static void QueryXmlWithNameSpace()
        {

            var document = XDocument.Load("fuel4.xml");

            var ns = (XNamespace)"http://pluralsight.com/cars/20216";
            var ex = (XNamespace)"http://pluralsight.com/cars/20216/ex";

            var query =
                    from element in document.Element(ns + "Cars")?.Elements(ex + "Car") 
                                                ?? Enumerable.Empty<XElement>()
                        //where element.Attribute("Manufacturer").Value == "BMW"
                    where element.Attribute("Manufacturer")?.Value == "BMW"
                    select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
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
