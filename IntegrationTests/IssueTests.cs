using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;
using VotingSystem.Utils;

namespace IntegrationTests
{
    internal static class IssueTests
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
                " (8) Genereate issue serial\n" +
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
                    '1' => DbInitializers.LoadTestData,
                    '2' => RunAllIssueTests,
                    '3' => TestAddIssue,
                    '4' => TestDeleteIssue,
                    '5' => TestGetIssues,
                    '6' => TestAddIssueDuplicateSerial,
                    '7' => TestAddIssueDuplicateTitle,
                    '8' => TestGenerateIssueSerial,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllIssueTests = new()
        {
            TestAddIssue,
            TestDeleteIssue,
            TestGetIssues,
            TestAddIssueDuplicateSerial,
            TestAddIssueDuplicateTitle,
            TestGenerateIssueSerial
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
            Console.WriteLine("    Testing add issue");
            if (!DbInitializers.ClearTestData())
                return false;
            var issue = TestData.issue.Build();

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
            Console.WriteLine("    Testing delete issue");
            if (!DbInitializers.LoadTestData())
                return false;
            var issue = TestData.issue.Build();
            
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
            Console.WriteLine("    Testing get issue");
            if (!DbInitializers.LoadTestData())
                return false;
            var issue = TestData.issue.Build();
            
            var fromDb = BallotIssue.Accessor.GetBallotIssues();
            if (!fromDb.Exists(x => x.SerialNumber == issue.SerialNumber))
            {
                Console.WriteLine("(F) Get ballot-issue failed: issue not returned");
            }

            Console.WriteLine("(S) Get ballot-issue success");
            return true;
        }

        public static bool TestAddIssueDuplicateSerial()
        {
            Console.WriteLine("    Testing add issue w/ duplicate serial");
            if (!DbInitializers.LoadTestData())
                return false;
            var issue = TestData.issue
                .WithTitle("Different Title")
                .Build();

            bool coll = BallotIssue.Accessor.AddIssue(issue);

            if (!coll)
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate serial failed: collision not detected");
                return false;
            }

            Console.WriteLine("(S) Add ballot-issue w/ duplicate serial success");
            return true;
        }

        public static bool TestAddIssueDuplicateTitle()
        {
            Console.WriteLine("    Testing Add issue duplicate title");
            if (!DbInitializers.LoadTestData())
                return false;
            var issue = TestData.issue
                .WithSerialNumber("45781269")
                .Build();

            bool coll = BallotIssue.Accessor.AddIssue(issue);

            if (!coll)
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate title failed: collision not detected");
                return false;
            }
            if (BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot-issue w/ duplicate title failed: issue added anyway");
                return false;
            }

            Console.WriteLine("(S) Add ballot-issue w/ duplicate title success");
            return true;
        }
        
        public static bool TestGenerateIssueSerial()
        {
            Console.WriteLine("    Testing issue serial generator");
            if (!DbInitializers.LoadTestData())
                return false;
            string serial = BallotIssue.Accessor.GetSerial();
            if (!Validation.IsValidSerialNumber(serial) || serial[0] != 'I')
            {
                Console.WriteLine($@"(F) Failed generate issue serial: '{serial}'");
                return false;
            }
            Console.WriteLine("(S) Generate issue serial success");
            return true;
        }
    }
}
