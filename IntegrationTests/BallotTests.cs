using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;

namespace IntegrationTests
{
	internal class BallotTests
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
                " (6) Add duplicate serial\n" +
                " (7) Add double vote\n" +
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
                    '2' => RunAllBallotTests,
                    '3' => TestAddBallot,
                    '4' => TestDeleteBallot,
                    '5' => TestGetBallot,
                    '6' => TestAddDuplicateBallotSerial,
                    '7' => TestAddDoubleBallot,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllBallotTests = new List<Func<bool>>()
        {
            TestAddBallot,
            TestDeleteBallot,
            TestGetBallot,
            TestAddDuplicateBallotSerial,
            TestAddDoubleBallot
        };

        public static bool RunAllBallotTests()
        {
            DbInitializers.ResetDb();
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
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V02222222")
                .WithEmail("xmail@laim.net")
                .WithUsername("BallotTestUN01")
                .WithPassword("asihWAj3@as")
                .WithFirstName("BallotTestFNAA")
                .WithLastName("BallotTestLNAB")
                .Build();
            Voter.Accessor.AddUser(voter);

            BallotIssue issue = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I02222222")
                .WithTitle("BallotTestIssueAA")
                .WithOptions("AA", "AB", "AC")
                .WithStartDate(new DateTime(2022, 5, 1))
                .WithEndDate(new DateTime(2022, 6, 2))
                .WithDescription("Test ballot issue for add ballot")
                .Build();
            BallotIssue.Accessor.AddIssue(issue);

            Ballot ballot = new Ballot.BallotBuilder()
                .WithVoter("V02222222")
                .WithIssue("I02222222")
                .WithChoice(1)
                .WithSerialNumber("B02222222")
                .Build();

            Console.WriteLine("    Testing add ballot");

            if ( Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Add admin failed: serial in use before addition (reset db?)");
                return false;
            }

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
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V01111111")
                .WithEmail("xmail14@laim.net")
                .WithUsername("BallotTestUN02")
                .WithPassword("asihWAj3@as")
                .WithFirstName("BallotTestFNBA")
                .WithLastName("BallotTestLNBB")
                .Build();
            Voter.Accessor.AddUser(voter);

            BallotIssue issue = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I01111111")
                .WithTitle("BallotTestIssueAB")
                .WithOptions("AX", "AY")
                .WithStartDate(new DateTime(2022, 5, 1))
                .WithEndDate(new DateTime(2022, 6, 2))
                .WithDescription("Test ballot issue for delete ballot")
                .Build();
            BallotIssue.Accessor.AddIssue(issue);

            Ballot ballot = new Ballot.BallotBuilder()
                .WithVoter("V01111111")
                .WithIssue("I01111111")
                .WithChoice(1)
                .WithSerialNumber("B01111111")
                .Build();


            Console.WriteLine("    Testing delete ballot");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Delete ballot failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Ballot.Accessor.AddBallot(ballot);

            if (!Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Delete ballot failed: serial/username not in use after addition");
                return false;
            }

            if (collision)
            {
                Console.WriteLine("(F) Delete ballot failed: collision (reset db?)");
                return false;
            }

            //Console.WriteLine($@"    Deleting admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

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
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V03333333")
                .WithEmail("asdkn@laim.net")
                .WithUsername("BallotTestUN03")
                .WithPassword("asihWAj3@as")
                .WithFirstName("BallotTestFNCA")
                .WithLastName("BallotTestLNCB")
                .Build();
            Voter.Accessor.AddUser(voter);

            BallotIssue issue = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I03333333")
                .WithTitle("BallotTestIssueAC")
                .WithOptions("T", "R")
                .WithStartDate(new DateTime(2022, 5, 1))
                .WithEndDate(new DateTime(2022, 6, 2))
                .WithDescription("Test ballot issue for get ballot")
                .Build();
            BallotIssue.Accessor.AddIssue(issue);

            Ballot ballot = new Ballot.BallotBuilder()
                .WithVoter("V03333333")
                .WithIssue("I03333333")
                .WithChoice(1)
                .WithSerialNumber("B03333333")
                .Build();


            Console.WriteLine("    Testing get ballot");

            if (Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Get ballot failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Ballot.Accessor.AddBallot(ballot);
            if (collision)
            {
                Console.WriteLine("(F) Get ballot failed: collison (reset db?)");
                return false;
            }
            if (!Ballot.Accessor.IsSerialInUse(ballot.SerialNumber))
            {
                Console.WriteLine("(F) Get ballot failed: serial not in use after addition");
                return false;
            }

            //Console.WriteLine($@"    Getting admin with u: '{admin.Username}', p:'{admin.Password}'");
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

            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V04444444")
                .WithEmail("asdsdakjasd@laim.net")
                .WithUsername("BallotTestUN04")
                .WithPassword("asihWAj3@as")
                .WithFirstName("BallotTestFNTA")
                .WithLastName("BallotTestLNXB")
                .Build();
            bool coll1 = Voter.Accessor.AddUser(voter);

            BallotIssue issue = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I04444444")
                .WithTitle("BallotTestIssueAJ")
                .WithOptions("Left", "Right")
                .WithStartDate(new DateTime(2022, 5, 1))
                .WithEndDate(new DateTime(2022, 6, 2))
                .WithDescription("Test ballot issue for add double ballot")
                .Build();
            bool coll2 = BallotIssue.Accessor.AddIssue(issue);

            Ballot ballot1 = new Ballot.BallotBuilder()
                .WithVoter("V04444444")
                .WithIssue("I04444444")
                .WithChoice(1)
                .WithSerialNumber("B04444444")
                .Build();

            Ballot ballot2 = new Ballot.BallotBuilder()
                .WithVoter("V04444444")
                .WithIssue("I04444444")
                .WithChoice(2)
                .WithSerialNumber("B04444445")
                .Build();

            if (coll1 || coll2)
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: pre collision (reset db?)");
                return false;
            }

            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || !BallotIssue.Accessor.IsSerialInUse(issue.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: voter/issue serial not in use after addition (reset db?)");
                return false;
            }


            if (Ballot.Accessor.IsSerialInUse(ballot1.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Ballot.Accessor.AddBallot(ballot1);
            if (collision)
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: pre-collision (reset db?)");
                return false;
            }

            bool collision2 = Ballot.Accessor.AddBallot(ballot2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: collision not detected");
                return false;
            }

            if (Ballot.Accessor.IsSerialInUse(ballot2.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate voter & issue failed: ballot added anyway");
                return false;
            }

            Console.WriteLine("(S) Add ballot w/ duplicate voter & issue success");
            return true;
        }

        public static bool TestAddDuplicateBallotSerial()
        {
            Voter voter1 = new Voter.VoterBuilder()
                .WithSerialNumber("V05555555")
                .WithEmail("dgasd@laim.net")
                .WithUsername("BallotTestUN07")
                .WithPassword("asihWAj3@as")
                .WithFirstName("BallotTestFNEA")
                .WithLastName("BallotTestLNEB")
                .Build();
            bool coll1 = Voter.Accessor.AddUser(voter1);

            Voter voter2 = new Voter.VoterBuilder()
                .WithSerialNumber("V05555556")
                .WithEmail("asdknasd@laim.net")
                .WithUsername("BallotTestUN08")
                .WithPassword("asihWAj3@as")
                .WithFirstName("BallotTestFNFA")
                .WithLastName("BallotTestLNFB")
                .Build();
            bool coll2 = Voter.Accessor.AddUser(voter2);

            BallotIssue issue1 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I05555555")
                .WithTitle("BallotTestIssueAF")
                .WithOptions("Left", "Right")
                .WithStartDate(new DateTime(2022, 5, 1))
                .WithEndDate(new DateTime(2022, 6, 2))
                .WithDescription("Test ballot issue for add duplicate serial ballot")
                .Build();
            bool coll3 = BallotIssue.Accessor.AddIssue(issue1);

            BallotIssue issue2 = new BallotIssue.BallotIssueBuilder()
                .WithSerialNumber("I05555556")
                .WithTitle("BallotTestIssueAG")
                .WithOptions("Left", "Right")
                .WithStartDate(new DateTime(2022, 5, 1))
                .WithEndDate(new DateTime(2022, 6, 2))
                .WithDescription("Test ballot issue for add duplicate serial ballot")
                .Build();
            bool coll4 = BallotIssue.Accessor.AddIssue(issue2);

            if (coll1 || coll2 
                ||!Voter.Accessor.IsSerialInUse(voter1.SerialNumber)
                || !Voter.Accessor.IsSerialInUse(voter2.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate serial failed: voter not added (reset db?)");
            }

            if (coll3 || coll4
                || !BallotIssue.Accessor.IsSerialInUse(issue1.SerialNumber)
                || !BallotIssue.Accessor.IsSerialInUse(issue2.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate serial failed: issue not added (reset db?)");
            }

            Ballot ballot1 = new Ballot.BallotBuilder()
                .WithVoter("V05555555")
                .WithIssue("I05555555")
                .WithChoice(1)
                .WithSerialNumber("B05555555")
                .Build();

            Ballot ballot2 = new Ballot.BallotBuilder()
                .WithVoter("V05555556")
                .WithIssue("I05555556")
                .WithChoice(0)
                .WithSerialNumber("B05555555")
                .Build();

            Console.WriteLine("    Testing add ballot w/ duplicate serial");

            if (Ballot.Accessor.IsSerialInUse(ballot1.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate serial failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Ballot.Accessor.AddBallot(ballot1);
            if (collision)
            {
                Console.WriteLine("(F) Add ballot w/ duplicate serial failed: pre-collision (reset db?)");
                return false;
            }
            if (!Ballot.Accessor.IsSerialInUse(ballot2.SerialNumber))
            {
                Console.WriteLine("(F) Add ballot w/ duplicate serial failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Ballot.Accessor.AddBallot(ballot2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add ballot w/ duplicate serial failed: collision not detected");
                return false;
            }

            Console.WriteLine("(S) Add ballot w/ duplicate serialsuccess");
            return true;
        }
    }
}

