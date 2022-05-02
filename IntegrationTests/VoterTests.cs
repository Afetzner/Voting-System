using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;
using VotingSystem.Controller;
using VotingSystem.Utils;

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
                " (8) Add duplicate email\n" +
                " (9) Generate voter serial\n" +
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
                    '2' => RunAllVoterTests,
                    '3' => TestAddVoter,
                    '4' => TestDeleteVoter,
                    '5' => TestGetVoter,
                    '6' => TestAddDuplicateVoterUsername,
                    '7' => TestAddDuplicateVoterSerial,
                    '8' => TestAddDuplicateVoterEmail,
                    '9' => TestGenerateVoterSerial,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllVoterTests = new()
        {
            TestAddVoter,
            TestDeleteVoter,
            TestGetVoter,
            TestAddDuplicateVoterSerial,
            TestAddDuplicateVoterUsername,
            TestAddDuplicateVoterEmail,
            TestGenerateVoterSerial
        };

        public static bool RunAllVoterTests()
        {
            DbInitializers.ResetDb();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllVoterTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail\n\n");
            return fail == 0;
        }

        public static bool TestAddVoter()
        {
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V77777777")
                .WithUsername("testVoterUsername1110")
                .WithPassword("testVoterPass1!")
                .WithEmail("124dasr@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter");
            //Console.WriteLine($@"Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Add voter failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add voter failed: username in use before addition (reset db?)");
                return false;
            }
            bool collision = Voter.Accessor.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add voter failed: collison (reset db?)");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || !Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add voter failed: serial/username not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add voter success");
            return true;
        }

        public static bool TestDeleteVoter()
        {
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V88888888")
                .WithUsername("warningTestVoterUsername332")
                .WithPassword("testVoterPass1!")
                .WithEmail("123dfs1@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing delete voter");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Delete voter failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Delete voter failed: username in use before addition (reset db?)");
                return false;
            }

            bool collision = Voter.Accessor.AddUser(voter);

            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || !Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Delete voter failed: serial/username not in use after addition");
                return false;
            }

            if (collision)
            {
                Console.WriteLine("(F) Delete voter failed: collision (reset db?)");
                return false;
            }

            //Console.WriteLine($@"    Deleting voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            Voter.Accessor.DeleteUser(voter.SerialNumber);
            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber) 
                || Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Delete voter failed: serial/username in use after deletion");
                return false;
            }

            Console.WriteLine("(S) Delete voter success");
            return true;
        }

        public static bool TestGetVoter()
        {
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V99999999")
                .WithUsername("testVoterUsername0123")
                .WithPassword("testVoterPass1!")
                .WithEmail("874@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing get voter");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Get voter failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Get voter failed: username in use before addition (reset db?)");
                return false;
            }
            bool collision = Voter.Accessor.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Get voter failed: collison (reset db?)");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || !Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Get voter failed: serial not in use after addition");
                return false;
            }

            //Console.WriteLine($@"    Getting voter with u: '{voter.Username}', p:'{voter.Password}'");
            Voter fromDb = Voter.Accessor.GetUser(voter.Username, voter.Password);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get voter failed: null output");
                return false;
            }
            if (fromDb.Username != voter.Username ||
                fromDb.Password != voter.Password ||
                fromDb.Email != voter.Email ||
                fromDb.FirstName != voter.FirstName ||
                fromDb.LastName != voter.LastName ||
                fromDb.SerialNumber != voter.SerialNumber)
            {
                Console.WriteLine("(F) Get voter failed: mismatch:");
                Console.WriteLine($@"Original: u:'{voter.Username}', p:'{voter.Password}', e:'{voter.Email}', f:'{voter.FirstName}', l:'{voter.LastName}', s:'{voter.SerialNumber}'");
                Console.WriteLine($@"Original: u:'{fromDb.Username}', p:'{fromDb.Password}', e:'{fromDb.Email}' ,f:'{fromDb.FirstName}', l:'{fromDb.LastName}', s:'{fromDb.SerialNumber}'");
                return false;
            }

            Console.WriteLine("(S) Get voter success");
            return true;
        }

        public static bool TestAddDuplicateVoterUsername()
        {
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V66666666")
                .WithUsername("testVoterUsername22")
                .WithPassword("testVoterPass1!")
                .WithEmail("0111@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Voter voter2 = new Voter.VoterBuilder()
                .WithSerialNumber("V55555555")
                .WithUsername("testVoterUsername22")
                .WithPassword("testVoterPass1!")
                .WithEmail("123d@gmail.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter w/ duplicate username");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || Voter.Accessor.IsSerialInUse(voter2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Voter.Accessor.IsUsernameInUse(voter.Username)
                || Voter.Accessor.IsUsernameInUse(voter2.Username))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: username in use before addition (reset db?)");
                return false;
            }
            bool collision = Voter.Accessor.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                ||!Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial/username not in use after addition");
                return false;
            }

            bool collision2 = Voter.Accessor.AddUser(voter2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: collision not detected");
                return false;
            }

            if (Voter.Accessor.IsSerialInUse(voter2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial/username of duplicate added ");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter username success");
            return true;
        }

        public static bool TestAddDuplicateVoterSerial()
        {
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V44444444")
                .WithUsername("testVoterUsername267")
                .WithPassword("testVoterPass1!")
                .WithEmail("ax1@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Voter voter2 = new Voter.VoterBuilder()
                .WithSerialNumber("V44444444")
                .WithUsername("testVoterUsername159")
                .WithPassword("testVoterPass1!")
                .WithEmail("z74@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter w/ duplicate serial");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                ||Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial/username in use before addition (reset db?)");
                return false;
            }

            bool collision = Voter.Accessor.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || !Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Voter.Accessor.AddUser(voter2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: collision not detected");
                return false;
            }

            if (Voter.Accessor.IsUsernameInUse(voter2.Username))
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter serial success");
            return true;
        }

        public static bool TestAddDuplicateVoterEmail()
        {
            Voter voter = new Voter.VoterBuilder()
                .WithSerialNumber("V22222222")
                .WithUsername("testVoterUsername660")
                .WithPassword("testVoterPass1!")
                .WithEmail("alex@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Voter voter2 = new Voter.VoterBuilder()
                .WithSerialNumber("V33333333")
                .WithUsername("testVoterUsername412")
                .WithPassword("testVoterPass1!")
                .WithEmail("alex@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add voter w/ duplicate email");
            //Console.WriteLine($@"    Adding voter with s: '{voter.SerialNumber}', u: '{voter.Username}', p:'{voter.Password}'");

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || Voter.Accessor.IsSerialInUse(voter2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter email failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Voter.Accessor.IsUsernameInUse(voter.Username)
                || Voter.Accessor.IsUsernameInUse(voter2.Username))
            {
                Console.WriteLine("(F) Add duplicate voter email failed: username in use before addition (reset db?)");
                return false;
            }

            bool collision = Voter.Accessor.AddUser(voter);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate voter email failed: pre-collision (reset db?)");
                return false;
            }
            if (!Voter.Accessor.IsSerialInUse(voter.SerialNumber)
                || !Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add duplicate voter email failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Voter.Accessor.AddUser(voter2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate voter email failed: collision not detected");
                return false;
            }

            if (Voter.Accessor.IsSerialInUse(voter2.SerialNumber) 
                || Voter.Accessor.IsUsernameInUse(voter2.Username))
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter email success");
            return true;
        }

        public static bool TestGenerateVoterSerial()
        {
            Console.WriteLine("    Testing voter serial generator");
            string serial = Voter.Accessor.GetSerial();
            if (!Validation.IsValidSerialNumber(serial) || serial[0] != 'V')
            {
                Console.WriteLine($@"(F) Failed generate issue serial: '{serial}'");
                return false;
            }
            Console.WriteLine("(S) Generate issue serial success");
            return true;
        }

    }
}
