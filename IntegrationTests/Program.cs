using System;
using IntegrationTests.Interactive;

namespace IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Integration Testing Console:");
            if (!Menu.ConnectionTestMenu())
                return;

            bool success = true;
            while (success)
            {
                var groupSelection = Menu.SelectTestGroupMenu();
                var actionSelection = groupSelection();
                success = actionSelection();
            }
            Console.WriteLine("\nExiting...");
        }
    }
}
