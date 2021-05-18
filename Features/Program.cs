using System;
using System.Collections.Generic;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee[] developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Scott" },
                new Employee { Id = 2, Name = "Chris" }
            };

            List<Employee> sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex" }
            };

            foreach (var person in developers)
            {
                Console.WriteLine(person.Name);
            }

            foreach (var person in sales)
            {
                Console.WriteLine(person.Name);
            }

            IEnumerable<Employee> processors = new Employee[]
            {
                new Employee { Id = 4, Name ="Steve" }
            };

            foreach (var person in processors)
            {
                Console.WriteLine(person.Name);
            }
            //var enumerator = developers.GetEnumerator();
            //-------------------------------------------------
            // IEnumerator / GetEnumerator
            //-------------------------------------------------
            //IEnumerator<Employee> enumerator = processors.GetEnumerator();
            IEnumerator<Employee> enumerator = sales.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }
        }
    }
}
