using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;
using VotingSystem.Utils;

namespace IntegrationTests
{
    internal static class VoterTests
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
                " (6) Get non-existent voter\n" +
                " (7) Get non-voter (admin)\n" +
                " (8) Add duplicate username\n" +
                " (9) Add dupicate serial\n" +
                " (a) Add duplicate email\n" +
                " (b) Generate voter serial\n" +
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
                    '1' => DbInitializers.LoadIntTestData,
                    '2' => RunAllVoterTests,
                    '3' => TestAddVoter,
                    '4' => TestDeleteVoter,
                    '5' => TestGetVoter,
                    '6' => TestGetNonExistentVoter,
                    '7' => TestGetNonVoter,
                    '8' => TestAddDuplicateVoterUsername,
                    '9' => TestAddDuplicateVoterSerial,
                    'a' => TestAddDuplicateVoterEmail,
                    'b' => TestGenerateVoterSerial,
                    _ => Menu.Exit,
                };
            }
        }

        public static List<Func<bool>> AllVoterTests = new()
        {
            TestAddVoter,
            TestDeleteVoter,
            TestGetVoter,
            TestGetNonExistentVoter,
            TestGetNonVoter,
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
            Console.WriteLine("    Testing add voter");
            DbInitializers.ResetDb();
            Voter v = new TestData().voter.Build();

            bool collision = Voter.Accessor.AddUser(v);
            if (collision)
            {
                Console.WriteLine("(F) Add voter failed: collison");
                return false;
            }

            if (!Voter.Accessor.IsSerialInUse(v.SerialNumber)
                || !Voter.Accessor.IsUsernameInUse(v.Username))
            {
                Console.WriteLine("(F) Add voter failed: serial/username not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add voter success");
            return true;
        }

        public static bool TestDeleteVoter()
        {
            Console.WriteLine("    Testing delete voter");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var voter = new TestData().voter.Build();

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
            Console.WriteLine("    Testing get voter");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var voter = new TestData().voter.Build();

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

        public static bool TestGetNonExistentVoter() 
        {
            Console.WriteLine("    Testing get non-existent voter");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var v = Voter.Accessor.GetUser("neverAUserName", "NeverAPassW0rd!");
            if (v != null)
            {
                Console.WriteLine("(F) Get non-existent voter failed: non null");
                Console.WriteLine($@"    s: '{v.SerialNumber}', f: '{v.FirstName}', l: '{v.LastName}', u: '{v.Username}', p: '{v.Password}'");
                return false;
            }
            Console.WriteLine("(S) Get non-existent voter success");
            return true;
        }

        public static bool TestGetNonVoter()
        {
            Console.WriteLine("    Testing get non-voter (admin)");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var admin = new TestData().admin.Build();

            var v = Voter.Accessor.GetUser(admin.Username, admin.Password);
            if (v != null)
            {
                Console.WriteLine("(F) Get non-existent voter failed: non null");
                return false;
            }
            Console.WriteLine("(S) Get non-existent voter success");
            return true;
        }

        public static bool TestAddDuplicateVoterUsername()
        {
            Console.WriteLine("    Testing add voter w/ duplicate username");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var voter = new TestData().voter
                .WithSerialNumber("V85461234")
                .WithEmail("otheremail@hotmail.com")
                .Build();

            bool coll = Voter.Accessor.AddUser(voter);

            if (!coll)
            {
                Console.WriteLine("(F) Add duplicate voter username failed: collision not detected");
                return false;
            }

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate voter username failed: serial/username of duplicate added ");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter username success");
            Voter.Accessor.DeleteUser(voter.SerialNumber);
            return true;
        }

        public static bool TestAddDuplicateVoterSerial()
        {
            Console.WriteLine("    Testing add voter w/ duplicate serial");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var voter = new TestData().voter
                .WithUsername("anotherUsername")
                .WithEmail("AcompleatlySeperateEmail")
                .Build();

            bool collision2 = Voter.Accessor.AddUser(voter);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: collision not detected");
                return false;
            }

            if (Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter serial success");
            Voter.Accessor.DeleteUser(voter.SerialNumber);
            return true;
        }

        public static bool TestAddDuplicateVoterEmail()
        {
            Console.WriteLine("    Testing add voter w/ duplicate email");
            if (!DbInitializers.LoadIntTestData())
                return false;
            var voter = new TestData().voter
                .WithUsername("UsernameOther")
                .WithSerialNumber("V96514567")
                .Build();

            
            bool coll = Voter.Accessor.AddUser(voter);
            if (!coll)
            {
                Console.WriteLine("(F) Add duplicate voter email failed: collision not detected");
                return false;
            }

            if (Voter.Accessor.IsSerialInUse(voter.SerialNumber) 
                || Voter.Accessor.IsUsernameInUse(voter.Username))
            {
                Console.WriteLine("(F) Add duplicate voter serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate voter email success");
            Voter.Accessor.DeleteUser(voter.SerialNumber);

            return true;
        }

        public static bool TestGenerateVoterSerial()
        {
            Console.WriteLine("    Testing voter serial generator");
            DbInitializers.LoadIntTestData();
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
