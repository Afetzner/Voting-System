using System;
using System.Collections.Generic;
using VotingSystem.Accessor;

namespace IntegrationTests.Interactive
{
    internal class Menu
    {
        public static bool ConnectionTestMenu()
        {
            Console.Write("Connection to DB: ");
            bool success = false;
            int tries = 0;
            while (tries < 3)
            {
                success = DbConnecter.TestConnection();
                tries++;
                Console.WriteLine(success ? "success" : "failed");
                if (success || !TryAgainMenu())
                    break;
            }

            Console.Write(tries == 3 ? "Forced exit\n" : "");
            return success;
        }

        private static bool TryAgainMenu()
        {
            Console.Write("Try again? (Y/N): ");
            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    'y' => true,
                    'Y' => true,
                    '1' => true,
                    _ => false
                };
            }
        }

        public static bool Exit()
        {
            return false;
        }

        public static Func<bool> MetaExit()
        {
            return Exit;
        }

        public static Func<Func<bool>> SelectTestGroupMenu()
        {
            Console.WriteLine("\nSelect a group to test");
            Console.WriteLine(
                " (0) Initialize Db\n" +
                " (1) Auto all tests\n" + 
                " (2) Admin\n" +
                " (3) Voter\n" +
                " (4) Ballot-Issue\n" +
                " (5) Ballot\n" + 
                " (6) Results\n" +
                " (*) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => DbInitializers.DbInitMenu,
                    '1' => MetaAllTests,
                    '2' => AdminTests.AdminTestMenu,
                    '3' => VoterTests.VoterTestMenu,
                    '4' => IssueTests.IssueTestMenu,
                    '5' => BallotTests.BallotTestMenu,
                    _ => MetaExit
                };
            }
        }

        private static List<List<Func<bool>>> tests = new List<List<Func<bool>>>()
        {
            VoterTests.AllVoterTests,
            AdminTests.AllAdminTests,
            IssueTests.AllIssueTests,
            BallotTests.AllBallotTests
        };

        private static Func<bool> MetaAllTests()
        {
            int total = 0;
            int fail = 0;

            Console.WriteLine("Running all integration tests...");
            Console.WriteLine("Reseting DB");
            DbInitializer.ResetDbTables();
            DbInitializer.LoadDummyDataFromSql();

            foreach (var testSuite in tests)
            {
                foreach (var test in testSuite)
                {
                    total++;
                    if (!test())
                        fail++;
                }
            }

            Console.WriteLine("\n\nTesting done:");
            Console.WriteLine($@"{fail}/{total} failed, {total - fail}/{total} passed");

            return Exit;
        }
    }
}
