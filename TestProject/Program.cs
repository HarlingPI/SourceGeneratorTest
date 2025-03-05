using SourceGeneratorInCSharp;
namespace TestProject
{
    [AutoToString]
    public partial class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var person = new Person { Name = "John", Age = 30 };
            Console.WriteLine(person);

            Console.WriteLine(HelloWorld.SayHello());

            Console.Read();
        }
    }
}
