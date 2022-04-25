using System;
using System.IO;
using VotingSystem.Model;
using VotingSystem.Controller;

namespace IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Integration Testing Console:");
            Console.WriteLine(ConnectionTestPrompt());
            var selection = SelectGroupOption();
            switch (selection)
            {
                case OuterOptions.Reset:
                    Reset();
                    break;
                case OuterOptions.Admin:
                    SelectAdminOption();
                    break;
                case OuterOptions.Voter:
                    SelectVoterOption();
                    break;
                case OuterOptions.BallotIssue:
                    SelectBallotIssue_Option();
                    break;
                case OuterOptions.IssueOption:
                    SelectIssueOption_Option();
                    break;
                case OuterOptions.Results:
                    SelectResultOption();
                    break;
                case OuterOptions.AutoTest:
                    AutoTestEveryThing();
                    break;
                default:
                    Console.WriteLine("Exiting");
                    return;
            }


        }

        private static bool ConnectionTestPrompt()
        {
            Console.Write("Connection to DB: ");
            bool success = false;
            int tries = 0;
            while (tries < 3)
            {
                success = DbConnecter.TestConnection();
                tries++;
                Console.WriteLine( success ? "success" : "failed");
                if (success || !TryAgainPrompt())
                    break;
            }

            Console.Write(tries == 3 ? "Forced exit\n" : "");
            return success;
        }

        private static bool TryAgainPrompt()
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

        private enum OuterOptions
        {
            Reset,
            Admin,
            Voter,
            BallotIssue,
            IssueOption,
            Results,
            AutoTest,
            Exit
        }

        private static OuterOptions SelectGroupOption()
        {
            Console.WriteLine("Select a group to test");
            Console.WriteLine(" (0) Reset\n (1) Admin\n (2) Voter\n (3) Ballot-Issue\n (4) Ballot-Issue Option\n (5) Results\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => OuterOptions.Reset,
                    '1' => OuterOptions.Admin,
                    '2' => OuterOptions.Voter,
                    '3' => OuterOptions.BallotIssue,
                    '4' => OuterOptions.IssueOption,
                    '5' => OuterOptions.Results,
                    '6' => OuterOptions.AutoTest,
                    '7' => OuterOptions.Exit,
                    _ => OuterOptions.Exit
                };
            }
        }

        private static void Reset()
        {
            throw new NotImplementedException();
        }

        private static bool SelectAdminOption(bool endOnError = true)
        {
            var admin = new AdminBuilder()
                .WithSerialNumber("A77777777")
                .WithUsername("testAdminUsername")
                .WithPassword("testAdminPass1!")
                .Build();

            var other1 = new AdminBuilder()
                .WithSerialNumber("A77777777")
                .WithUsername("warningUsername")
                .WithPassword("testAdminPass1!")
                .Build();

            var other2 = new AdminBuilder()
                .WithSerialNumber("A99999999")
                .WithUsername("testAdminUsername")
                .WithPassword("testAdminPass1!")
                .Build();

            Console.WriteLine($@"Using admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");
           
            Console.Write("Serial in use already? (expect no) ");
            var inUse = Admin.DbController.IsSerialInUse(admin.SerialNumber);
            Console.WriteLine( inUse ? "yes" : "no");
            if (endOnError & inUse)
                return false;


            Console.WriteLine($"Getting admin w/ username '{admin.Username}' and password '{admin.Password}'");
            var fromDb = Admin.DbController.GetUser(admin.Username, admin.Password);
            Console.WriteLine($@"Gotten admin has s: '{fromDb.SerialNumber}', u: '{fromDb.Username}', p:'{fromDb.Password}'");
            if (endOnError
                & !string.Equals(admin.SerialNumber, fromDb.SerialNumber)
                & !string.Equals(admin.Username, fromDb.Username)
                & !string.Equals(admin.Password, fromDb.Password))
                return false;


            Console.WriteLine("Adding...");
            var collision = Admin.DbController.AddUser(admin);
            Console.WriteLine($@"Done. Collision (expect no): {(collision ? "yes" : "no")}");
            if (endOnError & collision)
                return false;

            Console.Write("Serial now in use? (expect yes) ");
            inUse = Admin.DbController.IsSerialInUse(admin.SerialNumber);
            Console.WriteLine(inUse ? "yes" : "no");
            if (endOnError & !inUse)
                return false;

            Console.WriteLine("Trying to add duplicate serial:");
            collision = Admin.DbController.AddUser(other1);
            Console.WriteLine($@"Done. Collision (expect yes): {(collision ? "yes" : "no")}");
            if (endOnError & !collision)
                return false;

            Console.WriteLine("Trying to add duplicate username:");
            collision = Admin.DbController.AddUser(other2);
            Console.WriteLine($@"Done. Collision (expect yes): {(collision ? "yes" : "no")}");
            if (endOnError & !collision)
                return false;

            Console.WriteLine("Trying to removing admin");
            Admin.DbController.DeleteUser(admin.SerialNumber);
            Console.WriteLine("Done.");

            Console.Write("Serial in use after? (expect no) ");
            inUse = Admin.DbController.IsSerialInUse(admin.SerialNumber);
            Console.WriteLine(inUse ? "yes" : "no");
            if (endOnError & inUse)
                return false;

            //TODO
            //Check result of getUser

            return true;
        }

        private static void SelectVoterOption()
        {
            throw new NotImplementedException();
        }

        private static void SelectBallotIssue_Option()
        {
            throw new NotImplementedException();
        }

        private static void SelectIssueOption_Option()
        {
            throw new NotImplementedException();
        }

        private static void SelectResultOption()
        {
            throw new NotImplementedException();
        }

        private static void AutoTestEveryThing()
        {
            throw new NotImplementedException();
        }
    }
}
