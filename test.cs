using System;

namespace SimpleProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare and initialize variables
            string name = "User";
            int number = 42;

            // Print a greeting
            Console.WriteLine($"Hello, {name}! Your favorite number is {number}.");
            
            // Perform a simple calculation
            int doubled = number * 2;
            Console.WriteLine($"Double of {number} is {doubled}.");
            
            // Wait for user input before closing
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}