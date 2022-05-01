using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;

namespace IntegrationTests
{
	internal class BallotTests
	{
        public static bool TestAddBallot()
        {
            Ballot ballot = new Ballot.BallotBuilder()
                .WithVoter("John")
                .WithIssue("Trump")
                .WithChoice(1)
                .WithSerialNumber("A22222222")
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
                Console.Write("(F) Add ballot failed: serial not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add ballot success");
            return true;
        }

        public static bool TestDeleteBallot()
        {
            Ballot ballot = new Ballot.BallotBuilder()
                .WithVoter("John")
                .WithIssue("Trump")
                .WithChoice(1)
                .WithSerialNumber("V11111111")
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
            Ballot ballot = new Ballot.BallotBuilder()
                .WithVoter("John")
                .WithIssue("Trump")
                .WithChoice(1)
                .WithSerialNumber("R33333333")
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
                Console.Write("(F) Get ballot failed: serial not in use after addition");
                return false;
            }

            //Console.WriteLine($@"    Getting admin with u: '{admin.Username}', p:'{admin.Password}'");
            Ballot fromDb = Ballot.Accessor.GetBallot(ballot.VoterSerial, ballot.IssueSerial);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get ballot failed: null output");
                return false;
            }
            if (fromDb.VoterSerial != ballot.VoterSerial ||
                fromDb.SerialNumber != ballot.IssueSerial ||
                fromDb.Choice != ballot.Choice ||
                fromDb.SerialNumber != ballot.SerialNumber)
            {
                Console.WriteLine("(F) Get ballot failed: mismatch:");
                Console.WriteLine($@"Original: u:'{ballot.VoterSerial}', p:'{ballot.IssueSerial}', s:'{ballot.SerialNumber}'");
                Console.WriteLine($@"Original: u:'{fromDb.VoterSerial}', p:'{fromDb.IssueSerial}', s:'{fromDb.SerialNumber}'");
                return false;
            }

            Console.WriteLine("(S) Get ballot success");
            return true;
        }

        public static bool TestAddDuplicateBallotVoterIssueChoice()
        {
            Ballot ballot1 = new Ballot.BallotBuilder()
                .WithVoter("John")
                .WithIssue("Trump")
                .WithChoice(1)
                .WithSerialNumber("I44444444")
                .Build();

            Ballot ballot2 = new Ballot.BallotBuilder()
                .WithVoter("John")
                .WithIssue("Trump")
                .WithChoice(1)
                .WithSerialNumber("I44444445")
                .Build();

            Console.WriteLine("    Testing add ballot w/ duplicate serial");

            if (Ballot.Accessor.IsSerialInUse(ballot1.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate ballot voter-issue-choice failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Ballot.Accessor.AddBallot(ballot1);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate ballot voter-issue-choice failed: pre-collision (reset db?)");
                return false;
            }
            if (!Ballot.Accessor.IsSerialInUse(ballot2.SerialNumber))
            {
                Console.Write("(F) Add duplicate ballot voter-issue-choice failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Ballot.Accessor.AddBallot(ballot2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate ballot voter-issue-choice failed: collision not detected");
                return false;
            }

            Console.WriteLine("(S) Add duplicate ballot voter-issue-choice success");
            return true;
        }
    }
}

