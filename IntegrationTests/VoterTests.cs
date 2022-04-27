using System;
using VotingSystem.Model;
using IntegrationTests.Interactive;

namespace IntegrationTests
{
    internal class VoterTests
    {
        public static Func<bool> VoterTestMenu()
        {
            Console.WriteLine("Select a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all voter tests\n" +
                " (3) Add voter\n" +
                " (4) Delete voter\n" +
                " (5) Get voter\n" +
                " (6) Add duplicate username\n" +
                " (7) Add dupicate serial\n" +
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
                    '2' => AllVoterTests,
                    '3' => TestAddVoter,
                    '4' => TestDeleteVoter,
                    '5' => TestGetVoter,
                    '6' => TestAddDuplicateVoterUsername,
                    '7' => TestAddDuplicateVoterSerial,
                    '8' => Menu.Exit,
                    _ => Menu.Exit
                };
            }
        }

        public static bool AllVoterTests()
        {
            int fail = 0;
            if (!TestAddVoter())
                fail++;
            if (!TestDeleteVoter())
                fail++;
            if (!TestGetVoter())
                fail++;
            if (!TestAddDuplicateVoterSerial())
                fail++;
            if (!TestAddDuplicateVoterUsername())
                fail++;

            Console.WriteLine($@"{5 - fail} succeed, {fail} fail");
            return fail == 0;
        }

        public static bool TestAddVoter()
        {
            Voter voter = new VoterBuilder()
                .WithSerialNumber("V77777777")
                .WithUsername("testVoterUsername1110")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter");
            //Console.WriteLine($@"Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Add voter failed: serial in use before addition (reset db?)");
                return false;
            }
            bool collision = Voter.DbController.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add voter failed: collison (reset db?)");
                return false;
            }
            if (!Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.Write("(F) Add voter failed: serial not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add voter success");
            return true;
        }

        public static bool TestDeleteVoter()
        {
            Voter voter = new VoterBuilder()
                .WithSerialNumber("V88888888")
                .WithUsername("warningTestVoterUsername332")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing delete voter");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Delete voter failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Voter.DbController.AddUser(voter);

            if (!Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Delete voter failed: serial not in use after addition");
                return false;
            }

            if (collision)
            {
                Console.WriteLine("(F) Delete voter failed: collision (reset db?)");
                return false;
            }

            Console.WriteLine($@"    Deleting voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            Voter.DbController.DeleteUser(voter.SerialNumber);
            if (Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Delete voter failed: serial in use after deletion");
                return false;
            }

            Console.WriteLine("(S) Delete voter success");
            return true;
        }

        public static bool TestGetVoter()
        {
            Voter voter = new VoterBuilder()
                .WithSerialNumber("V99999999")
                .WithUsername("testVoterUsername0123")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing get voter");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Get voter failed: serial in use before addition (reset db?)");
                return false;
            }
            bool collision = Voter.DbController.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Get voter failed: collison (reset db?)");
                return false;
            }
            if (!Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.Write("(F) Get voter failed: serial not in use after addition");
                return false;
            }

            Console.WriteLine($@"    Getting voter with u: '{voter.Username}', p:'{voter.Password}'");
            Voter fromDb = Voter.DbController.GetUser(voter.Username, voter.Password);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get voter failed: null output");
                return false;
            }
            if (fromDb.Username != voter.Username ||
                fromDb.Password != voter.Password ||
                fromDb.FirstName != voter.FirstName ||
                fromDb.LastName != voter.LastName ||
                fromDb.SerialNumber != voter.SerialNumber)
            {
                Console.WriteLine("(F) Get voter failed: mismatch:");
                Console.WriteLine($@"Original: u:'{voter.Username}', p:'{voter.Password}', f:'{voter.FirstName}', l:'{voter.LastName}', s:'{voter.SerialNumber}'");
                Console.WriteLine($@"Original: u:'{fromDb.Username}', p:'{fromDb.Password}', f:'{fromDb.FirstName}', l:'{fromDb.LastName}', s:'{fromDb.SerialNumber}'");
                return false;
            }

            Console.WriteLine("(S) Get voter success");
            return true;
        }

        public static bool TestAddDuplicateVoterUsername()
        {
            Voter voter = new VoterBuilder()
                .WithSerialNumber("V66666666")
                .WithUsername("testVoterUsername22")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Voter voter2 = new VoterBuilder()
                .WithSerialNumber("V55555555")
                .WithUsername("testVoterUsername22")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter w/ duplicate username");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.DbController.IsSerialInUse(voter.SerialNumber)
                || Voter.DbController.IsSerialInUse(voter2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial in use before addition (reset db?)");
                return false;
            }
            bool collision = Voter.DbController.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.Write("(F) Add duplicate voter username failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Voter.DbController.AddUser(voter2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: collision not detected");
                return false;
            }

            if (Voter.DbController.IsSerialInUse(voter2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial of duplicate added ");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter username success");
            return true;
        }

        public static bool TestAddDuplicateVoterSerial()
        {
            Voter voter = new VoterBuilder()
                .WithSerialNumber("V44444444")
                .WithUsername("testVoterUsername267")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Voter voter2 = new VoterBuilder()
                .WithSerialNumber("V44444444")
                .WithUsername("testVoterUsername159")
                .WithPassword("testVoterPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter w/ duplicate serial");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Voter.DbController.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Voter.DbController.IsSerialInUse(voter.SerialNumber))
            {
                Console.Write("(F) Add duplicate voter username failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Voter.DbController.AddUser(voter2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: collision not detected");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter serial success");
            return true;
        }
    }
}
