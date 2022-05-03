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
                " (1) Load test data from sql\n" +
                " (2) Load int-test data\n" +
                " (3) Clear int-test data\n" + 
                " (*) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => ResetDb,
                    '1' => LoadTestDataFromSql,
                    '2' => LoadIntTestData,
                    '3' => ResetDb,
                    _ => Menu.Exit
                };
            }
        }

        public static bool ResetDb()
        {
            DbInitializer.ResetDbTables();
            return true;
        }

        public static bool LoadTestDataFromSql()
        {
            DbInitializer.LoadDummyDataFromSql();
            return true;
        }

        public static bool LoadIntTestData()
        {
            ResetDb();
            //Add voters
            bool coll = Voter.Accessor.AddUser(new TestData().voter.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: voter collision");
                Environment.Exit(1);
            }
            if (!Voter.Accessor.IsSerialInUse(new TestData().voter.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: voter serial not added");
                Environment.Exit(1);
            }

            //Add admins
            coll = Admin.Accessor.AddUser(new TestData().admin.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: admin collision");
                Environment.Exit(1);
            }
            if (!Voter.Accessor.IsSerialInUse(new TestData().admin.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: admin serial not added");
                Environment.Exit(1);
            }

            //Add issues
            coll = BallotIssue.Accessor.AddIssue(new TestData().issue.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: issue collision");
                Environment.Exit(1);
            }
            if (!BallotIssue.Accessor.IsSerialInUse(new TestData().issue.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: issue serial not added");
                Environment.Exit(1);
            }

            //Add ballots
            coll = Ballot.Accessor.AddBallot(new TestData().ballot.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: ballot collision");
                Environment.Exit(1);
            }
            if (!Ballot.Accessor.IsSerialInUse(new TestData().ballot.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: ballot serial not added");
                Environment.Exit(1);
            }

            return true;
        }
    }

    internal class TestData
    {

        public Voter.Builder voter = new Voter.Builder()
            .WithFirstName("IntTestVoter")
            .WithLastName("IntTestLastName")
            .WithUsername("IntTestUser1")
            .WithPassword("abcABC123!")
            .WithEmail("IntTestEmail1")
            .WithSerialNumber("V20000001");

        public Admin.Builder admin = new Admin.Builder()
            .WithFirstName("IntTestAdmin")
            .WithLastName("IntTestLastName")
            .WithUsername("IntTestAdmin1")
            .WithPassword("abcABC123!")
            .WithEmail("IntTestEmail3")
            .WithSerialNumber("A20000001");

        public BallotIssue.Builder issue = new BallotIssue.Builder()
            .WithTitle("IntTestIssue1")
            .WithDescription("Int-Test issue 1 desc")
            .WithStartDate(new DateTime(2022, 5, 1))
            .WithEndDate(new DateTime(2022, 5, 2))
            .WithOptions("Issue 1 - option 1", "Issue 1 - option 2", "Issue 1 - option 3")
            .WithSerialNumber("I30000001");

        public Ballot.Builder ballot = new Ballot.Builder()
            .WithVoter("V20000001")
            .WithIssue("I30000001")
            .WithChoice(0)
            .WithSerialNumber("B40000001");

    }
}
