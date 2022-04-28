using System;
using IntegrationTests.Interactive;
using VotingSystem.Controller;

namespace IntegrationTests
{
    public class DbInitializers
    {
        public static Func<bool> DbInitMenu()
        {
            Console.WriteLine("Select a group to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => ResetDb,
                    '1' => LoadTestData,
                    '2' => Menu.Exit,
                    _ => Menu.Exit
                };
            }
        }

        public static bool ResetDb()
        {
            DbInitializer.ResetDbTables();
            return true;
        }

        public static bool LoadTestData()
        {
            DbInitializer.LoadDummyData();
            return true;
        }
    }
}
