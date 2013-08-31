using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace ConsoleApplication3
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Concat(Name, ", Age: ", Age);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            QueryAsync().ContinueWith(_ => Console.WriteLine("Done"));
            Console.ReadLine();
        }

        private async static Task QueryAsync()
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=DapperTryDb;Integrated Security=true;Asynchronous Processing=True"))
            {
                await conn.OpenAsync();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    IEnumerable<Person> people = await conn.QueryAsync<Person>("SELECT * FROM People WHERE Age = @Age", new { Age = 17 }, trans);
                    foreach (Person person in people)
                    {
                        Console.WriteLine(person);
                    }
                }
            }
        }
    }
}
