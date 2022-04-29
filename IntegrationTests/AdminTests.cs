using VotingSystem.Model;
using IntegrationTests.Interactive;
using System;
using System.Collections.Generic;

namespace IntegrationTests
{
    internal class AdminTests
    {
        public static Func<bool> AdminTestMenu()
        {
            Console.WriteLine("Select a method to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Auto all admin tests\n" +
                " (3) Add admin\n" +
                " (4) Delete admin\n" +
                " (5) Get admin\n" +
                " (6) Add duplicate username\n" +
                " (7) Add dupicate serial\n" +
                " (8) Add duplicate email\n" +
                " (9) Exit\n");

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
                    '2' => RunAllAdminTests,
                    '3' => TestAddAdmin,
                    '4' => TestDeleteAdmin,
                    '5' => TestGetAdmin,
                    '6' => TestAddDuplicateAdminUsername,
                    '7' => TestAddDuplicateAdminSerial,
                    '8' => TestAddDuplicateAdminEmail,
                    _ => Menu.Exit
                };
            }
        }

        public static List<Func<bool>> AllAdminTests = new List<Func<bool>>()
        {
            TestAddAdmin,
            TestDeleteAdmin,
            TestGetAdmin,
            TestAddDuplicateAdminSerial,
            TestAddDuplicateAdminUsername,
            TestAddDuplicateAdminEmail
        };

        public static bool RunAllAdminTests()
        {
            DbInitializers.ResetDb();
            int fail = 0;
            int tot = 0;
            foreach (var test in AllAdminTests)
            {
                tot++;
                if (!test())
                    fail++;
            }

            Console.WriteLine($@"{tot - fail} succeed, {fail} fail\n\n");
            return fail == 0;
        }

        public static bool TestAddAdmin()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A77777777")
                .WithUsername("testAdminUsername4551")
                .WithPassword("testAdminPass1!")
                .WithEmail("123ytr@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add admin");
            //Console.WriteLine($@"Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Add admin failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Add admin failed: username in use before addition (reset db?)");
                return false;
            }
            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add admin failed: collison (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.Write("(F) Add admin failed: serial/username not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add admin success");
            return true;
        }

        public static bool TestDeleteAdmin()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A88888888")
                .WithUsername("warningTestAdminUsernameX72")
                .WithPassword("testAdminPass1!")
                .WithEmail("ppo@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing delete admin");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Delete admin failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Delete admin failed: username in use before addition (reset db?)");
                return false;
            }

            bool collision = Admin.Accessor.AddUser(admin);

            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Delete admin failed: serial/username not in use after addition");
                return false;
            }

            if (collision)
            {
                Console.WriteLine("(F) Delete admin failed: collision (reset db?)");
                return false;
            }

            //Console.WriteLine($@"    Deleting admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

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
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A99999999")
                .WithUsername("testAdminUsername123fg")
                .WithPassword("testAdminPass1!")
                .WithEmail("tyyt@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing get admin");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Get admin failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Get admin failed: username in use before addition (reset db?)");
                return false;
            }
            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Get admin failed: collison (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.Write("(F) Get admin failed: serial not in use after addition");
                return false;
            }

            //Console.WriteLine($@"    Getting admin with u: '{admin.Username}', p:'{admin.Password}'");
            Admin fromDb = Admin.Accessor.GetUser(admin.Username, admin.Password);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get admin failed: null output");
                return false;
            }
            if (fromDb.Username != admin.Username ||
                fromDb.Password != admin.Password ||
                fromDb.Email != admin.Email ||
                fromDb.FirstName != admin.FirstName ||
                fromDb.LastName != admin.LastName ||
                fromDb.SerialNumber != admin.SerialNumber)
            {
                Console.WriteLine("(F) Get admin failed: mismatch:");
                Console.WriteLine($@"Original: u:'{admin.Username}', p:'{admin.Password}', e:'{admin.Email}', f:'{admin.FirstName}', l:'{admin.LastName}', s:'{admin.SerialNumber}'");
                Console.WriteLine($@"Original: u:'{fromDb.Username}', p:'{fromDb.Password}', e:'{fromDb.Email}' ,f:'{fromDb.FirstName}', l:'{fromDb.LastName}', s:'{fromDb.SerialNumber}'");
                return false;
            }

            Console.WriteLine("(S) Get admin success");
            return true;
        }

        public static bool TestAddDuplicateAdminUsername()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A66666666")
                .WithUsername("testAdminUsername67112")
                .WithPassword("testAdminPass1!")
                .WithEmail("hgghh@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Admin admin2 = new AdminBuilder()
                .WithSerialNumber("A55555555")
                .WithUsername("testAdminUsername67112")
                .WithPassword("testAdminPass1!")
                .WithEmail("rrtfa@gmail.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add admin w/ duplicate username");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || Admin.Accessor.IsSerialInUse(admin2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate admin username failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Admin.Accessor.IsUsernameInUse(admin.Username)
                || Admin.Accessor.IsUsernameInUse(admin2.Username))
            {
                Console.WriteLine("(F) Add duplicate admin username failed: username in use before addition (reset db?)");
                return false;
            }
            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate admin username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.Write("(F) Add duplicate admin username failed: serial/username not in use after addition");
                return false;
            }

            bool collision2 = Admin.Accessor.AddUser(admin2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate admin username failed: collision not detected");
                return false;
            }

            if (Admin.Accessor.IsSerialInUse(admin2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate admin username failed: serial/username of duplicate added ");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin username success");
            return true;
        }

        public static bool TestAddDuplicateAdminSerial()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A44444444")
                .WithUsername("testAdminUsernametyytd")
                .WithPassword("testAdminPass1!")
                .WithEmail("1234lkk@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Admin admin2 = new AdminBuilder()
                .WithSerialNumber("A44444444")
                .WithUsername("testAdminUsername123f34")
                .WithPassword("testAdminPass1!")
                .WithEmail("gghma@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add admin w/ duplicate serial");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.WriteLine("(F) Add duplicate admin username failed: serial/username in use before addition (reset db?)");
                return false;
            }

            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate admin username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.Write("(F) Add duplicate admin username failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Admin.Accessor.AddUser(admin2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate admin serial failed: collision not detected");
                return false;
            }

            if (Admin.Accessor.IsUsernameInUse(admin2.Username))
            {
                Console.WriteLine("(F) Add duplicate admin serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin serial success");
            return true;
        }

        public static bool TestAddDuplicateAdminEmail()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A22222222")
                .WithUsername("testAdminUsernamesddcc")
                .WithPassword("testAdminPass1!")
                .WithEmail("alexR11@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Admin admin2 = new AdminBuilder()
                .WithSerialNumber("A33333333")
                .WithUsername("testAdminUsername8unjj")
                .WithPassword("testAdminPass1!")
                .WithEmail("alexR11@email.com")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add admin w/ duplicate email");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || Admin.Accessor.IsSerialInUse(admin2.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate admin email failed: serial in use before addition (reset db?)");
                return false;
            }
            if (Admin.Accessor.IsUsernameInUse(admin.Username)
                || Admin.Accessor.IsUsernameInUse(admin2.Username))
            {
                Console.WriteLine("(F) Add duplicate admin email failed: username in use before addition (reset db?)");
                return false;
            }

            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate admin email failed: pre-collision (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber)
                || !Admin.Accessor.IsUsernameInUse(admin.Username))
            {
                Console.Write("(F) Add duplicate admin email failed: serial not in use after addition");
                return false;
            }

            bool collision2 = Admin.Accessor.AddUser(admin2);
            if (!collision2)
            {
                Console.WriteLine("(F) Add duplicate admin email failed: collision not detected");
                return false;
            }

            if (Admin.Accessor.IsSerialInUse(admin2.SerialNumber)
                || Admin.Accessor.IsUsernameInUse(admin2.Username))
            {
                Console.WriteLine("(F) Add duplicate admin serial failed: duplicate entry added");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin email success");
            return true;
        }
    }
}

