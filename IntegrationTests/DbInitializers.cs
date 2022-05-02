using System;
using IntegrationTests.Interactive;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace IntegrationTests
{
    internal static class DbInitializers
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
            ClearTestData();
            //Add voters
            bool coll = Voter.Accessor.AddUser(TestData.voter.Build());
            if (coll)
            {
                Console.WriteLine("Could not load test data: voter collision");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(TestData.voter.SerialNumber))
            {
                Console.WriteLine("Could not load test data: voter serial not added");
                return false;
            }

            //Add admins
            coll = Admin.Accessor.AddUser(TestData.admin.Build());
            if (coll)
            {
                Console.WriteLine("Could not load test data: admin collision");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(TestData.admin.SerialNumber))
            {
                Console.WriteLine("Could not load test data: admin serial not added");
                return false;
            }

            //Add issues
            coll = BallotIssue.Accessor.AddIssue(TestData.issue.Build());
            if (coll)
            {
                Console.WriteLine("Could not load test data: issue collision");
                return false;
            }
            if (!BallotIssue.Accessor.IsSerialInUse(TestData.voter.SerialNumber))
            {
                Console.WriteLine("Could not load test data: issue serial not added");
                return false;
            }

            //Add ballots
            coll = Ballot.Accessor.AddBallot(TestData.ballot.Build());
            if (coll)
            {
                Console.WriteLine("Could not load test data: ballot collision");
                return false;
            }
            if (!Ballot.Accessor.IsSerialInUse(TestData.ballot.SerialNumber))
            {
                Console.WriteLine("Could not load test data: ballot serial not added");
                return false;
            }

            return true;
        }
    
        public static bool ClearTestData()
        {
            Voter.Accessor.DeleteUser(TestData.voter.SerialNumber);
            if (Voter.Accessor.IsSerialInUse(TestData.voter.SerialNumber))
            {
                Console.WriteLine("Clear test data error: voter serial still in use");
                return false;
            }

            Admin.Accessor.DeleteUser(TestData.admin.SerialNumber);
            if (Admin.Accessor.IsSerialInUse(TestData.admin.SerialNumber))
            {
                Console.WriteLine("Clear test data error: admin serial still in use");
                return false;
            }

            BallotIssue.Accessor.RemoveIssue(TestData.issue.SerialNumber);
            if (BallotIssue.Accessor.IsSerialInUse(TestData.issue.SerialNumber))
            {
                Console.WriteLine("Clear test data error: ballot-issue serial still in use");
                return false;
            }

            Ballot.Accessor.RemoveBallot(TestData.ballot.SerialNumber);
            if (Ballot.Accessor.IsSerialInUse(TestData.ballot.SerialNumber))
            {
                Console.WriteLine("Clear test data error: ballot serial still in use");
                return false;
            }

            return true;
        }
    }

    internal static class TestData
    {
        public static Voter.VoterBuilder voter = new Voter.VoterBuilder()
            .WithFirstName("IntTestVoter1")
            .WithLastName("IntTestLastName1")
            .WithUsername("IntTestUser1")
            .WithPassword("abcABC123!")
            .WithEmail("IntTestEmail1")
            .WithSerialNumber("V200000001");

        public static Admin.AdminBuilder admin = new Admin.AdminBuilder()
            .WithFirstName("IntTestAdmin1")
            .WithLastName("IntTestLastName1")
            .WithUsername("IntTestAdmin1")
            .WithPassword("abcABC123!")
            .WithEmail("IntTestEmail3")
            .WithSerialNumber("A200000001");

        public static BallotIssue.BallotIssueBuilder issue = new BallotIssue.BallotIssueBuilder()
            .WithTitle("IntTestIssue1")
            .WithDescription("Int-Test issue 1 desc")
            .WithStartDate(new DateTime(2022, 5, 1))
            .WithEndDate(new DateTime(2022, 5, 2))
            .WithOptions("Issue 1 - option 1", "Issue 1 - option 2", "Issue 1 - option 3")
            .WithSerialNumber("I30000001");

        public static Ballot.BallotBuilder ballot = new Ballot.BallotBuilder()
            .WithVoter(voter.SerialNumber)
            .WithIssue(issue.SerialNumber)
            .WithChoice(0)
            .WithSerialNumber("B40000001");

    }
}
