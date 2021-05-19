using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> square = x => x * x;
            Console.WriteLine(square(3));
            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine(add(3, 5));
            Func<int, int, int> subtract = (int x, int y) => {
                int temp = x - y;
                return temp;
            };
            Console.WriteLine(subtract(3, 5));


            Action<int> write = x => Console.WriteLine(x);

            Employee[] developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Scott" },
                new Employee { Id = 2, Name = "Chris" }
            };

            List<Employee> sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex" }
            };

            Console.WriteLine(sales.Count());

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

            Console.WriteLine("[Where - Named Method]=====================================");
            foreach (var employee in developers.Where(NameStartsWithS))
            {
                Console.WriteLine(employee.Name.ToString());
            }

            Console.WriteLine("[Where - Anonymous Method]=====================================");
            foreach (var employee in developers.Where(
                    delegate(Employee employee) {
                        return employee.Name.StartsWith("S");
                    }))
            {
                Console.WriteLine(employee.Name.ToString());
            }

            Console.WriteLine("[Where - Lambda Express]=====================================");
            foreach (var employee in developers.Where(e => e.Name.StartsWith("S")))
            {
                Console.WriteLine(employee.Name.ToString());
            }


        }

        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
