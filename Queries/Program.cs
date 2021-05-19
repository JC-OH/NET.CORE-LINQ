using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight",   Rating = 8.9f, Year = 2008 },
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010 },
                new Movie { Title = "Casablanca",        Rating = 8.5f, Year = 1942 },
                new Movie { Title = "Star Wars V",       Rating = 8.7f, Year = 1980 }
            };
            Console.WriteLine("====================================================");
            var emptyQuery = Enumerable.Empty<Movie>();

            /*try
            {
                emptyQuery = movies.Where(m => m.Year > 2000).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(emptyQuery.Count());*/

            var query = movies.Where(m => m.Year > 2000);

            foreach (var movie in query)
            {
                Console.WriteLine(movie.Title);
            }
            Console.WriteLine("====================================================");
            var query2 = movies.Filter(m => m.Year > 2000);
            foreach (var movie in query2)
            {
                Console.WriteLine(movie.Title);
            }

            Console.WriteLine("====================================================");
            /*var query3 = movies.Filter(m => m.Year > 2000)
                                .Take(1);*/
            /*var query3 = movies.Filter(m => m.Year > 2000);
            Console.WriteLine(query3.Count());*/

            var query3 = movies.Filter(m => m.Year > 2000).ToList();
            Console.WriteLine(query3.Count());
            var enumerator = query3.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

        }
    }
}
