using RazorTemplatingSample.Web;

namespace RazorTemplatingSample.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Person[] model = new[]
            {
                new Person { Name = "foo" },
                new Person { Name = "bar" }
            };

            System.Console.WriteLine(Templater.Run(model));
            System.Console.ReadLine();
        }
    }
}
