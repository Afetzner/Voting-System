using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;
namespace IntegrationTests
{
    internal class IssueTests
    {
        public static Func<bool> IssueTestMenu()
        {
            Console.WriteLine("Select a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all issue tests\n" +
                " (3) Add issue\n" +
                " (4) Delete issue\n" +
                " (5) Get issues\n" +
                " (6) Add duplicate serial\n" +
                " (7) Add dupicate title\n" +
                " (8) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => DbInitializers.ResetDb,
                    '1' => DbInitializers.LoadTestData,
                    '2' => RunAllIssueTests,
                    '3' => TestAddIssue,
                    '4' => TestDeleteIssue,
                    '5' => TestGetIssues,
                    '6' => TestAddIssueDuplicateSerial,
                    '7' => TestAddIssueDuplicateTitle,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllIssueTests = new List<Func<bool>>()
        {
            TestAddIssue,
            TestDeleteIssue,
            TestGetIssues,
            TestAddIssueDuplicateSerial,
            TestAddIssueDuplicateTitle
        };

        public static bool RunAllIssueTests()
        {
            DbInitializers.ResetDb();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllIssueTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail\n\n");
            return fail == 0;
        }

        public static bool TestAddIssue()
        {
            var issue = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I00000000")
                .WithStartDate(new DateTime(2022, 04, 28))
                .WithEndDate(new DateTime(2022, 05, 28))
                .WithTitle("TestIssue000")
                .WithOptions("A", "B", "C")
                .Build();

            Console.WriteLine("    Testing add issue");
            if (BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue failed: serial in use before addition (reset DB?)");
                return false;
            }
            
            bool collision = BallotIssue.Accessor.AddIssue(issue);

            if (collision)
            {
                Console.WriteLine("(F) Add ballot-issue failed: collision (reset DB?)");
            }
            if (!BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue failed: serial not in use after addition (reset DB?)");
                return false;
            }

            Console.WriteLine("(S) Add ballot-issue success");
            return true;
        }

        public static bool TestDeleteIssue()
        {
            var issue = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I111111111")
                .WithStartDate(new DateTime(2022, 04, 28))
                .WithEndDate(new DateTime(2022, 05, 28))
                .WithTitle("TestIssue001")
                .WithOptions("X", "Y")
                .Build();

            Console.WriteLine("    Testing delete issue");
            if (BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Delete ballot-issue failed: serial in use before addition (reset DB?)");
                return false;
            }

            bool collision = BallotIssue.Accessor.AddIssue(issue);

            if (collision)
            {
                Console.WriteLine("(F) Delete ballot-issue failed: collision (reset DB?)");
            }
            if (!BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Delete ballot-issue failed: serial not in use after addition (reset DB?)");
                return false;
            }

            BallotIssue.Accessor.RemoveIssue(issue.SerialNumber);

            if (BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Delete ballot-issue failed: serial in use after deletion (reset DB?)");
                return false;
            }

            Console.WriteLine("(S) Delete ballot-issue success");
            return true;
        }

        public static bool TestGetIssues()
        {
            var issue1 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I22222222")
                .WithStartDate(new DateTime(2022, 06, 11))
                .WithEndDate(new DateTime(2022, 07, 01))
                .WithTitle("TestIssue002")
                .WithOptions("Alpha", "Beta", "Gamma", "Delta")
                .Build();

            var issue2 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I33333333")
                .WithStartDate(new DateTime(2022, 01, 01))
                .WithEndDate(new DateTime(2022, 12, 31))
                .WithTitle("TestIssue003")
                .WithOptions("Wumbo", "Wuble-u")
                .Build();

            Console.WriteLine("    Testing get issue");
            if (BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber)
                || BallotIssue.Accessor.IsSerialInUse(issue2.SerialNumber))
            {
                Console.WriteLine("(F) Get ballot-issue failed: serial in use before addition (reset DB?)");
                return false;
            }

            bool collision1 = BallotIssue.Accessor.AddIssue(issue1);
            bool collision2 = BallotIssue.Accessor.AddIssue(issue2);

            if (collision1 || collision2)
            {
                Console.WriteLine("(F) Get ballot-issue failed: collision (reset DB?)");
            }
            if (!BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber)||
                !BallotIssue.Accessor.IsSerialInUse(issue2.SerialNumber))
            {
                Console.WriteLine("(F) Get ballot-issue failed: serial not in use after addition");
                return false;
            }

            var fromDb = BallotIssue.Accessor.GetBallotIssues();
            if (!fromDb.Exists(x => x.SerialNumber == issue1.SerialNumber)
                || !fromDb.Exists(x => x.SerialNumber == issue2.SerialNumber))
            {
                Console.WriteLine("(F) Get ballot-issue faile: issues not returned");
            }

            Console.WriteLine("(S) Get ballot-issue success");
            return true;
        }

        public static bool TestAddIssueDuplicateSerial()
        {
            var issue1 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I44444444")
                .WithStartDate(new DateTime(2022, 07, 28))
                .WithEndDate(new DateTime(2022, 12, 09))
                .WithTitle("TestIssue004")
                .WithOptions("1", "2", "3")
                .Build();

            var issue2 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I44444444")
                .WithStartDate(new DateTime(2022, 08, 11))
                .WithEndDate(new DateTime(2022, 08, 16))
                .WithTitle("TestIssue005")
                .WithOptions("One", "Two")
                .Build();

            Console.WriteLine("    Testing add issue w/ duplicate serial");
            if (BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: serial in use before addition (reset DB?)");
                return false;
            }

            bool collision = BallotIssue.Accessor.AddIssue(issue1);

            if (collision)
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: pre-collision (reset DB?)");
            }
            if (!BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: serial not in use after addition");
                return false;
            }

            collision = BallotIssue.Accessor.AddIssue(issue2);

            if (!collision)
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: collision not detected");
                return false;
            }

            Console.WriteLine("(S) Add ballot-issue w/ duplicate serial success");
            return true;
        }

        public static bool TestAddIssueDuplicateTitle()
        {
            var issue1 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I66666666")
                .WithStartDate(new DateTime(2022, 01, 05))
                .WithEndDate(new DateTime(2022, 02, 09))
                .WithTitle("TestIssue006")
                .WithOptions("1", "2", "3")
                .Build();

            var issue2 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I77777777")
                .WithStartDate(new DateTime(2022, 11, 08))
                .WithEndDate(new DateTime(2022, 12, 08))
                .WithTitle("TestIssue006")
                .WithOptions("One", "Two")
                .Build();

            Console.WriteLine("    Testing add issue w/ duplicate title");
            if (BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber)
                || BallotIssue.Accessor.IsSerialInUse(issue2.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate title failed: serial in use before addition (reset DB?)");
                return false;
            }

            bool collision = BallotIssue.Accessor.AddIssue(issue1);

            if (collision)
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate title failed: pre-collision (reset DB?)");
            }
            if (!BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: serial not in use after addition");
                return false;
            }

            collision = BallotIssue.Accessor.AddIssue(issue2);

            if (!collision)
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: collision not detected");
                return false;
            }
            if (BallotIssue.Accessor.IsSerialInUse(issue2.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: issue added anyway");
                return false;
            }

            Console.WriteLine("(S) Add ballot-issue w/ duplicate serial success");
            return true;
        }
    }
}
