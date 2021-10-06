using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8090();
            Welcome7731();
            Console.ReadKey();
        }
        static partial void Welcome7731();
        private static void Welcome8090()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
