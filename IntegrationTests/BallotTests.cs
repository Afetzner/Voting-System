using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;
using VotingSystem.Utils;

namespace IntegrationTests
{
	internal static class BallotTests
	{
        public static Func<bool> BallotTestMenu()
        {
            Console.WriteLine("Select a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all ballot tests\n" +
                " (3) Add ballot\n" +
                " (4) Delete ballot\n" +
                " (5) Get ballot\n" +
                " (6) Add double vote\n" +
                " (7) Generate ballot serial\n" +
                " (*) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => TestDataLoader.UnloadTestData,
                    '1' => TestDataLoader.LoadIntTestData,
                    '2' => RunAllBallotTests,
                    '3' => TestAddBallot,
                    '4' => TestDeleteBallot,
                    '5' => TestGetBallot,
                    '6' => TestAddDoubleBallot,
                    '7' => TestGenerateBallotSerial,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllBallotTests = new ()
        {
            TestAddBallot,
            TestDeleteBallot,
            TestGetBallot,
            TestAddDoubleBallot,
            TestGenerateBallotSerial
        };

        public static bool RunAllBallotTests()
        {
            TestDataLoader.UnloadTestData();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllBallotTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail\n\n");
            return fail == 0;
        }

        public static bool TestAddBallot()
        {
            Console.WriteLine("    Testing add ballot");
            TestDataLoader.UnloadTestData();
            var voter = new TestData().voter.Build();
            var issue = new TestData().issue.Build();
            var ballot = new TestData().ballot.Build();

            Voter.Accessor.AddUser(voter);
            BallotIssue.Accessor.AddIssue(issue);

            bool collision = Ballot.Accessor.AddBallot(ballot);
            if (collision)
            {
                Console.WriteLine("(F) Add ballot failed: collison (reset db?)");
                return false;
            }
            if (!Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot failed: serial not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add ballot success");
            return true;
        }

        public static bool TestDeleteBallot()
        {
            Console.WriteLine("    Testing delete ballot");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var ballot = new TestData().ballot.Build();

            Ballot.Accessor.RemoveBallot(ballot.SerialNumber);
            if (Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Delete ballot failed: serial in use after deletion");
                return false;
            }

            Console.WriteLine("(S) Delete ballot success");
            return true;
        }

        public static bool TestGetBallot()
        {
            Console.WriteLine("    Testing get ballot");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var ballot = new TestData().ballot.Build();

            Ballot fromDb = Ballot.Accessor.GetBallot(ballot.VoterSerial, ballot.IssueSerial);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get ballot failed: null output");
                return false;
            }
            if (fromDb.VoterSerial  != ballot.VoterSerial ||
                fromDb.IssueSerial  != ballot.IssueSerial ||
                fromDb.Choice       != ballot.Choice ||
                fromDb.SerialNumber != ballot.SerialNumber)
            {
                Console.WriteLine("(F) Get ballot failed: mismatch:");
                Console.WriteLine($@"Original: u:'{ballot.VoterSerial}', p:'{ballot.IssueSerial}', s:'{ballot.SerialNumber}', c:'{ballot.Choice}'");
                Console.WriteLine($@"Original: u:'{fromDb.VoterSerial}', p:'{fromDb.IssueSerial}', s:'{fromDb.SerialNumber}', c:'{fromDb.Choice}'");
                return false;
            }

            Console.WriteLine("(S) Get ballot success");
            return true;
        }

        public static bool TestAddDoubleBallot()
        {
            Console.WriteLine("    Testing add ballot w/ duplicate voter & issue");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var ballot = new TestData().ballot
                .WithSerialNumber("B12695478")
                .WithChoice(1)
                .Build();

            bool collision2 = Ballot.Accessor.AddBallot(ballot);
            if (!collision2)
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: collision not detected");
                return false;
            }

            if (Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: ballot added anyway");
                return false;
            }

            Console.WriteLine("(S) Add ballot w/ duplicate voter & issue success");
            return true;
        }

        public static bool TestGenerateBallotSerial()
        {
            Console.WriteLine("    Testing issue serial generator");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            string serial = Ballot.Accessor.GetSerial();
            if (!Validation.IsValidSerialNumber(serial) || serial[0] != 'B')
            {
                Console.WriteLine($@"(F) Failed generate issue serial: '{serial}'");
                return false;
            }
            Console.WriteLine("(S) Generate issue serial success");
            return true;
        }
    }
}

