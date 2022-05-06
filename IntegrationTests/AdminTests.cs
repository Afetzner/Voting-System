using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;
using VotingSystem.Utils;

namespace IntegrationTests
{
    internal static class AdminTests
    {
        public static Func<bool> AdminTestMenu()
        {
            Console.WriteLine("\nSelect a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all admin tests\n" +
                " (3) Add admin\n" +
                " (4) Delete admin\n" +
                " (5) Get admin\n" +
                " (6) Get non-existent admin\n" +
                " (7) Get non-admin (voter)\n" +
                " (8) Add duplicate username\n" +
                " (9) Add dupicate serial\n" +
                " (a) Add duplicate email\n" +
                " (b) Generate Admin serial\n" +
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
                    '2' => RunAllAdminTests,
                    '3' => TestAddAdmin,
                    '4' => TestDeleteAdmin,
                    '5' => TestGetAdmin,
                    '6' => TestGetNonExistentAdmin,
                    '7' => TestGetNonAdmin,
                    '8' => TestAddDuplicateAdminUsername,
                    '9' => TestAddDuplicateAdminSerial,
                    'a' => TestAddDuplicateAdminEmail,
                    'b' => TestGenerateAdminSerial,
                    _ => Menu.Exit
                };
            }
        }

        public static List<Func<bool>> AllAdminTests = new()
        {
            TestAddAdmin,
            TestDeleteAdmin,
            TestGetAdmin,
            TestGetNonExistentAdmin,
            TestGetNonAdmin,
            TestAddDuplicateAdminSerial,
            TestAddDuplicateAdminUsername,
            TestAddDuplicateAdminEmail,
            TestGenerateAdminSerial
        };

        public static bool RunAllAdminTests()
        {
            TestDataLoader.UnloadTestData();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllAdminTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail");
            Console.WriteLine("\n");
            return fail == 0;
        }

        public static bool TestAddAdmin()
        {
            Console.WriteLine("    Testing add admin");
            TestDataLoader.UnloadTestData();
            Admin v = new TestData().admin.Build();

            bool collision = Admin.Accessor.AddUser(v);
            if (collision)
            {
                Console.WriteLine("(F) Add admin failed: collison");
                return false;
            }

            if (!Admin.Accessor.IsSerialInUse(v.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(v.Username))
            {
                Console.WriteLine("(F) Add admin failed: serial/username not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add admin success");
            return true;
        }

        public static bool TestDeleteAdmin()
        {
            Console.WriteLine("    Testing delete admin");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var admin = new TestData().admin.Build();

            Admin.Accessor.DeleteUser(admin.SerialNumber);
            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Delete admin failed: serial/username in use after deletion");
                return false;
            }

            Console.WriteLine("(S) Delete admin success");
            return true;
        }

        public static bool TestGetAdmin()
        {
            Console.WriteLine("    Testing get admin");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var admin = new TestData().admin;

            Admin fromDb = Admin.Accessor.GetUser(admin.Username, admin.Password);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get admin failed: null output");
                return false;
            }
            if (fromDb.Username != admin.Username ||
                fromDb.Password != "placeholderPassword1!" ||
                fromDb.Email != admin.Email ||
                fromDb.FirstName != admin.FirstName ||
                fromDb.LastName != admin.LastName ||
                fromDb.SerialNumber != admin.SerialNumber)
            {
                Console.WriteLine("(F) Get admin failed: mismatch:");
                Console.WriteLine($@"Expected: u:'{admin.Username}', p:'{admin.Password}', e:'{admin.Email}', f:'{admin.FirstName}', l:'{admin.LastName}', s:'{admin.SerialNumber}'");
                Console.WriteLine($@"Actual: u:'{fromDb.Username}', p:'{fromDb.Password}', e:'{fromDb.Email}' ,f:'{fromDb.FirstName}', l:'{fromDb.LastName}', s:'{fromDb.SerialNumber}'");
                return false;
            }

            Console.WriteLine("(S) Get admin success");
            return true;
        }

        public static bool TestGetNonExistentAdmin()
        {
            Console.WriteLine("    Testing get non-existent admin");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var v = Admin.Accessor.GetUser("neverAUserName", "NeverAPassW0rd!");
            if (v != null)
            {
                Console.WriteLine("(F) Get non-existent admin failed: non null");
                Console.WriteLine($@"    s: '{v.SerialNumber}', f: '{v.FirstName}', l: '{v.LastName}', u: '{v.Username}', p: '{v.Password}'");
                return false;
            }
            Console.WriteLine("(S) Get non-existent admin success");
            return true;
        }

        public static bool TestGetNonAdmin()
        {
            Console.WriteLine("    Testing get non-admin (admin)");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var voter = new TestData().voter.Build();

            var v = Admin.Accessor.GetUser(voter.Username, voter.Password);
            if (v != null)
            {
                Console.WriteLine("(F) Get non-existent admin failed: non null");
                return false;
            }
            Console.WriteLine("(S) Get non-existent admin success");
            return true;
        }

        public static bool TestAddDuplicateAdminUsername()
        {
            Console.WriteLine("    Testing add admin w/ duplicate username");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var admin = new TestData().admin
                .WithSerialNumber("V85461234")
                .WithEmail("otheremail@hotmail.com")
                .Build();

            bool coll = Admin.Accessor.AddUser(admin);

            if (!coll)
            {
                Console.WriteLine("(F) Add duplicate admin username failed: collision not detected");
                return false;
            }

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate admin username failed: serial/username of duplicate added ");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin username success");
            Admin.Accessor.DeleteUser(admin.SerialNumber);
            return true;
        }

        public static bool TestAddDuplicateAdminSerial()
        {
            Console.WriteLine("    Testing add admin w/ duplicate serial");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var admin = new TestData().admin
                .WithUsername("anotherUsername")
                .WithEmail("AcompleatlySeperateEmail")
                .Build();

            bool collision2 = Admin.Accessor.AddUser(admin);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate admin serial failed: collision not detected");
                return false;
            }

            if (Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Add duplicate admin serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin serial success");
            Admin.Accessor.DeleteUser(admin.SerialNumber);
            return true;
        }

        public static bool TestAddDuplicateAdminEmail()
        {
            Console.WriteLine("    Testing add admin w/ duplicate email");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            var admin = new TestData().admin
                .WithUsername("YetAnotherUsername")
                .WithSerialNumber("V96514567")
                .Build();


            bool coll = Admin.Accessor.AddUser(admin);
            if (!coll)
            {
                Console.WriteLine("(F) Add duplicate admin email failed: collision not detected");
                return false;
            }

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Add duplicate admin serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin email success");
            Admin.Accessor.DeleteUser(admin.SerialNumber);
            return true;
        }

        public static bool TestGenerateAdminSerial()
        {
            Console.WriteLine("    Testing admin serial generator");
            if (!TestDataLoader.LoadIntTestData())
                return false;
            string serial = Admin.Accessor.GetSerial();
            if (!Validation.IsValidSerialNumber(serial) || serial[0] != 'A')
            {
                Console.WriteLine($@"(F) Failed generate issue serial: '{serial}'");
                return false;
            }
            Console.WriteLine("(S) Generate admin serial success");
            return true;
        }
    }
}

