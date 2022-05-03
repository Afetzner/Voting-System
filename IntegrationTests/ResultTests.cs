using System;
using System.Collections.Generic;
using VotingSystem.Accessor;
using VotingSystem.Controller;
using VotingSystem.Model;
using IntegrationTests.Interactive;


namespace IntegrationTests
{
    internal static class ResultTests
    {
        public static Func<bool> ResultTestMenu()
        {
            Console.WriteLine("Select a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all voter tests\n" +
                " (*) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => DbInitializers.ResetDb,
                    '1' => DbInitializers.LoadIntTestData,
                    '2' => RunAllVoterTests,

                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllResultTests = new()
        {

        };

        public static bool RunAllVoterTests()
        {
            DbInitializers.ResetDb();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllResultTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail\n\n");
            return fail == 0;
        }

        public static bool TestGetIssues()
        {

            return true;
        }
    }
}
