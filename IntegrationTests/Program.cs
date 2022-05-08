using System;
using IntegrationTests.Interactive;
using System.Collections.Generic;
using VotingSystem.Model;

namespace IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(System.Configuration.ConfigurationManager.ConnectionStrings[0]);
            Console.WriteLine("Integration Testing Console");
            if (!Menu.ConnectionTestMenu())
                return;

            bool success;
            do
            {
                var groupSelection = Menu.SelectTestGroupMenu();
                var actionSelection = groupSelection();
                success = actionSelection();
            } while (success);

            Console.WriteLine("\nExiting...");
        }
    }
}
