using System;
using VotingSystem.Controller;

namespace VotingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(ValidationUtils.IsValidDate("2999-12-31"));
            Console.WriteLine(ValidationUtils.IsValidDate("2000-01-01"));
        }
    }
}
