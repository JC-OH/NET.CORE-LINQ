using DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Implementation
{
    public class Customer : ICustomer
    {
        public void CreateCustomer()
        {
            Console.WriteLine("Creating a customer with concrete class injected using constructor injection");
        }
    }
}
