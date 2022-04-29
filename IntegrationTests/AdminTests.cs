using System;
using VotingSystem.Model;
using IntegrationTests.Interactive;

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
                    '2' => AllAdminTests,
                    '3' => TestAddAdmin,
                    '4' => TestDeleteAdmin,
                    '5' => TestGetAdmin,
                    '6' => TestAddDuplicateAdminUsername,
                    '7' => TestAddDuplicateAdminSerial,
                    '8' => Menu.Exit,
                    _ => Menu.Exit
                };
            }
        }

        public static bool AllAdminTests()
        {
            int fail = 0;
            if (!TestAddAdmin())
                fail++;
            if (!TestDeleteAdmin())
                fail++;
            if (!TestGetAdmin())
                fail++;
            if (!TestAddDuplicateAdminSerial())
                fail++;
            if (!TestAddDuplicateAdminUsername())
                fail++;

            Console.WriteLine($@"{5 - fail} succeed, {fail} fail");
            return fail == 0;
        }

        public static bool TestAddAdmin()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A77777777")
                .WithUsername("testAdminUsername1110")
                .WithPassword("testAdminPass1!")
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
            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add admin failed: collison (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.Write("(F) Add admin failed: serial not in use after addition");
                return false;
            }

            Console.WriteLine("(S) Add admin success");
            return true;
        }

        public static bool TestDeleteAdmin()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A88888888")
                .WithUsername("warningTestAdminUsername332")
                .WithPassword("testAdminPass1!")
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

            bool collision = Admin.Accessor.AddUser(admin);

            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Delete admin failed: serial not in use after addition");
                return false;
            }
                
            if (collision)
            {
                Console.WriteLine("(F) Delete admin failed: collision (reset db?)");
                return false;
            }

            Console.WriteLine($@"    Deleting admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            Admin.Accessor.DeleteUser(admin.SerialNumber);
            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Delete admin failed: serial in use after deletion");
                return false;
            }

            Console.WriteLine("(S) Delete admin success");
            return true;
        }
    
        public static bool TestGetAdmin()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A99999999")
                .WithUsername("testAdminUsername0123")
                .WithPassword("testAdminPass1!")
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
            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Get admin failed: collison (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.Write("(F) Get admin failed: serial not in use after addition");
                return false;
            }

            Console.WriteLine($@"    Getting admin with u: '{admin.Username}', p:'{admin.Password}'");
            Admin fromDb = (Admin)Admin.Accessor.GetUser(admin.Username, admin.Password);

            if (fromDb == null)
            {
                Console.WriteLine("(F) Get admin failed: null output");
                return false;
            }
            if (fromDb.Username != admin.Username ||
                fromDb.Password != admin.Password ||
                fromDb.FirstName != admin.FirstName ||
                fromDb.LastName != admin.LastName ||
                fromDb.SerialNumber != admin.SerialNumber)
            {
                Console.WriteLine("(F) Get admin failed: mismatch:");
                Console.WriteLine($@"Original: u:'{admin.Username}', p:'{admin.Password}', f:'{admin.FirstName}', l:'{admin.LastName}', s:'{admin.SerialNumber}'");
                Console.WriteLine($@"Original: u:'{fromDb.Username}', p:'{fromDb.Password}', f:'{fromDb.FirstName}', l:'{fromDb.LastName}', s:'{fromDb.SerialNumber}'");
                return false;
            }

            Console.WriteLine("(S) Get admin success");
            return true;
        }

        public static bool TestAddDuplicateAdminUsername()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A66666666")
                .WithUsername("testAdminUsername22")
                .WithPassword("testAdminPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Admin admin2 = new AdminBuilder()
                .WithSerialNumber("A55555555")
                .WithUsername("testAdminUsername22")
                .WithPassword("testAdminPass1!")
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
            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate admin username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.Write("(F) Add duplicate admin username failed: serial not in use after addition");
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
                Console.WriteLine("(F) Add duplicate admin username failed: serial of duplicate added ");
                return false;
            }

            Console.WriteLine("(S) Add duplicate admin username success");
            return true;
        }

        public static bool TestAddDuplicateAdminSerial()
        {
            Admin admin = new AdminBuilder()
                .WithSerialNumber("A44444444")
                .WithUsername("testAdminUsername267")
                .WithPassword("testAdminPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Admin admin2 = new AdminBuilder()
                .WithSerialNumber("A44444444")
                .WithUsername("testAdminUsername159")
                .WithPassword("testAdminPass1!")
                .WithFirstName("Jane")
                .WithLastName("Doe")
                .Build();

            Console.WriteLine("    Testing add admin w/ duplicate serial");
            //Console.WriteLine($@"    Adding admin with s: '{admin.SerialNumber}', u: '{admin.Username}', p:'{admin.Password}'");

            if (Admin.Accessor.IsSerialInUse(admin.SerialNumber))
            {
                Console.WriteLine("(F) Add duplicate admin username failed: serial in use before addition (reset db?)");
                return false;
            }

            bool collision = Admin.Accessor.AddUser(admin);
            if (collision)
            {
                Console.WriteLine("(F) Add duplicate admin username failed: pre-collision (reset db?)");
                return false;
            }
            if (!Admin.Accessor.IsSerialInUse(admin.SerialNumber))
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

            Console.WriteLine("(S) Add duplicate admin serial success");
            return true;
        }
    }
}
