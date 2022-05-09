using System;
using IntegrationTests.Interactive;
using VotingSystem.Accessor;
using VotingSystem.Model;

namespace IntegrationTests
{
    internal static class TestDataLoader
    {
        public static Func<bool> DbInitMenu()
        {
            Console.WriteLine("Select a group to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load test data from sql\n" +
                " (2) Generate example data\n" +
                " (3) Load int-test data\n" +
                " (4) Clear int-test data\n" + 
                " (*) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => UnloadTestData,
                    '1' => LoadTestDataFromSql,
                    '2' => GenerateExampleData,
                    '3' => LoadIntTestData,
                    '4' => UnloadTestData,
                    _ => Menu.Exit
                };
            }
        }

        public static bool UnloadTestData()
        {
            //Unload voters
            Voter.Accessor.DeleteUser(new TestData().voter.SerialNumber);
            Voter.Accessor.DeleteUser(new TestData().voter2.SerialNumber);

            //Unload admins
            Admin.Accessor.DeleteUser(new TestData().admin.SerialNumber);

            //Unload issues
            BallotIssue.Accessor.RemoveIssue(new TestData().issue.SerialNumber);
            BallotIssue.Accessor.RemoveIssue(new TestData().issue2.SerialNumber);

            //Unload ballots
            Ballot.Accessor.RemoveBallot(new TestData().ballot.SerialNumber);
            Ballot.Accessor.RemoveBallot(new TestData().ballot2.SerialNumber);
            Ballot.Accessor.RemoveBallot(new TestData().ballot3.SerialNumber);
            return true;
        }

        public static bool ResetDb()
        {
            DbInitializer.ResetDbTables();
            return true;
        }

        public static bool GenerateExampleData()
        {
            int num;
            while (true) 
            {
                Console.WriteLine("How many voters?");
                string input = Console.ReadLine();
                try
                {
                    num = Convert.ToInt32(input);
                }
                catch (Exception)
                {
                    continue;
                }
                break;
            }
            Console.WriteLine($@"Generating {num} voters");
            DbInitializer.LoadDummyDataGenerated(num);
            Console.WriteLine("Done");
            return true;
        }

        public static bool LoadTestDataFromSql()
        {
            DbInitializer.LoadDummyDataFromSql();
            return true;
        }

        public static bool LoadIntTestData()
        {
            UnloadTestData();
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

        public static bool LoadIntTestDataForResultsViewer()
        {
            UnloadTestData();
            LoadIntTestData();
            //Add voters
            bool coll = Voter.Accessor.AddUser(new TestData().voter2.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: voter 2 collision");
                Environment.Exit(1);
            }
            if (!Voter.Accessor.IsSerialInUse(new TestData().voter2.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: voter 2 serial not added");
                Environment.Exit(1);
            }

            //Add issues
            coll = BallotIssue.Accessor.AddIssue(new TestData().issue2.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: issue 2 collision");
                Environment.Exit(1);
            }
            if (!BallotIssue.Accessor.IsSerialInUse(new TestData().issue2.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: issue 2 serial not added");
                Environment.Exit(1);
            }

            //Add ballots
            coll = Ballot.Accessor.AddBallot(new TestData().ballot2.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: ballot collision");
                Environment.Exit(1);
            }
            if (!Ballot.Accessor.IsSerialInUse(new TestData().ballot2.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: ballot 2 serial not added");
                Environment.Exit(1);
            }

            coll = Ballot.Accessor.AddBallot(new TestData().ballot3.Build());
            if (coll)
            {
                Console.WriteLine("(E) Could not load test data: ballot collision");
                Environment.Exit(1);
            }
            if (!Ballot.Accessor.IsSerialInUse(new TestData().ballot3.SerialNumber))
            {
                Console.WriteLine("(E) Could not load test data: ballot 3 serial not added");
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
            .WithEmail("IntTestEmail2")
            .WithSerialNumber("A20000001");

        public BallotIssue.Builder issue = new BallotIssue.Builder()
            .WithTitle("IntTestIssue1")
            .WithDescription("Int-Test issue 1 desc")
            .WithStartDate(new DateTime(2022, 5, 1))
            .WithEndDate(new DateTime(2022, 5, 2))
            .WithOptions("Issue 1 - option X", "Issue 1 - option Y", "Issue 1 - option Z")
            .WithSerialNumber("I30000001");

        //Voter 1, issue 1, choice 0
        public Ballot.Builder ballot = new Ballot.Builder()
            .WithVoter("V20000001")
            .WithIssue("I30000001")
            .WithChoice(0)
            .WithSerialNumber("B40000001");


        //Extra data for results viewer
        public Voter.Builder voter2 = new Voter.Builder()
            .WithFirstName("IntTestVoterTwo")
            .WithLastName("IntTestLastNameTwo")
            .WithUsername("IntTestUser2")
            .WithPassword("abcABC123!")
            .WithEmail("IntTestEmail3")
            .WithSerialNumber("V20000002");

        public BallotIssue.Builder issue2 = new BallotIssue.Builder()
            .WithTitle("IntTestIssue2")
            .WithDescription("Int-Test issue 2 desc")
            .WithStartDate(new DateTime(2022, 5, 1))
            .WithEndDate(new DateTime(2022, 5, 2))
            .WithOptions("Issue 2 - option A", "Issue 2 - option B", "Issue 2 - option C")
            .WithSerialNumber("I30000002");

        //Voter 1, issue 2, choice 1
        public Ballot.Builder ballot2 = new Ballot.Builder()
            .WithVoter("V20000001")
            .WithIssue("I30000002")
            .WithChoice(1)
            .WithSerialNumber("B40000002");

        //Voter 2, issue 1, choice 0
        public Ballot.Builder ballot3 = new Ballot.Builder()
            .WithVoter("V20000002")
            .WithIssue("I30000001")
            .WithChoice(0)
            .WithSerialNumber("B40000003");
    }
}
