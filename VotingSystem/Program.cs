using System;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
using System.Diagnostics.CodeAnalysis;
>>>>>>> 7d03c21 (fixed bad procedure variable names, fixed admin controller)
using MySql.Data.MySqlClient;
using VotingSystem.Controller;
using VotingSystem.Model;

=======
using VotingSystem.Controller;
>>>>>>> 2149257 (added validateDate tests)
=======
using MySql.Data.MySqlClient;
using VotingSystem.Controller;
using VotingSystem.Model;
>>>>>>> cd543ae (implimented stored procedures)

namespace VotingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            Voter me = new VoterBuilder()
                .WithLastName("Fetz")
                .WithFirstName("Alex")
                .WithMiddleName("N")
                .WithLicenseNumber("A12345678")
                .Build();

            bool success = DbConnecter.TestConnection();
            if (success)
            {
                Console.WriteLine("Success, adding...");
                try
                {
                    VoterAccessor.AddVoter(me);
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e);
                }
            }
            else 
                Console.WriteLine("Failure");
            
=======
            Console.WriteLine("Hello World!");
<<<<<<< HEAD
            Console.WriteLine(ValidationUtils.IsValidDate("2999-12-31"));
            Console.WriteLine(ValidationUtils.IsValidDate("2000-01-01"));
>>>>>>> 2149257 (added validateDate tests)
=======
>>>>>>> 98ec0b5 (minor change)
=======
            Voter me = new VoterBuilder()
                .WithUsername("JDoe11")
=======
            var voter = new VoterBuilder()
                .WithUsername("SarahTheFirst")
>>>>>>> 2443d3f (Fixed some procedures/accessors. Added helpful comments for upcomming accessors)
                .WithPassword("D0eN0ADear!")
                .WithLastName("Sandra")
                .WithFirstName("Sarah")
                .WithSerialNumber("V77124460")
                .Build();

<<<<<<< HEAD
<<<<<<< HEAD
            int myId = 0;
            try
            {
                myId = Voter.DbController.AddEntry(me);
                Console.WriteLine("Entry added");
            }
            catch (MySqlException e)
            {
                Console.WriteLine("1>" + e);
            }

            try
            {
                int dbId = Voter.DbController.GetId(me);
                Console.WriteLine("IDs are equal: " + (myId == dbId));
            }
            catch (MySqlException e)
            {
                Console.WriteLine("2>" + e);
                
            }

            try
            {
                Voter you = Voter.DbController.GetInfo(myId);
                Console.WriteLine("Voters are equal:" + (me == you));
            }
            catch (MySqlException e)
            {
                Console.WriteLine("3>" + e);

            }

            try
            {
                Voter.DbController.DeleteEntry(myId);
            }
            catch (MySqlException e)
            {
                Console.WriteLine("4>" + e);
            }
            Console.WriteLine("Done!");

>>>>>>> cd543ae (implimented stored procedures)
=======
>>>>>>> 1caf447 (Big remodel)
=======
            Console.WriteLine("Testing voter accessor");

            Console.WriteLine("In use (before): " + Voter.DbController.IsSerialInUse("V77124460"));
            Voter.DbController.AddUser(voter);
            Console.WriteLine("In use after): " + Voter.DbController.IsSerialInUse("V77124460"));

            var other = Voter.DbController.GetUser(voter.Username, voter.Password);
            Console.WriteLine($@"u: '{other.Username}', p: '{other.Password}', s: '{other.SerialNumber}', f: '{other.FirstName}', l: '{other.LastName}'");
                
            Voter.DbController.DeleteUser(voter.SerialNumber);
            Console.WriteLine("In use (after delete): " + Voter.DbController.IsSerialInUse("V77124460"));
            Console.WriteLine("Done");
<<<<<<< HEAD
>>>>>>> 2443d3f (Fixed some procedures/accessors. Added helpful comments for upcomming accessors)
=======

      


            var admin = new AdminBuilder()
                .WithUsername("BillyJoel")
                .WithPassword("WillyTheKid12#!")
                .WithSerialNumber("A78885510")
                .Build();

            Console.WriteLine("Testing admin accessor");

            Console.WriteLine("In use (before): " + Admin.DbController.IsSerialInUse("A78885510"));
            Admin.DbController.AddUser(admin);
            Console.WriteLine("In use (after): " + Admin.DbController.IsSerialInUse("A78885510"));

            var other2 = Admin.DbController.GetUser(admin.Username, admin.Password);
            Console.WriteLine($@"u: '{other2.Username}', p: '{other2.Password}', s: '{other2.SerialNumber}'");

            Admin.DbController.DeleteUser(admin.SerialNumber);
            Console.WriteLine("In use (after delete): " + Voter.DbController.IsSerialInUse("A78885510"));
            Console.WriteLine("Done");
<<<<<<< HEAD
>>>>>>> 7f03271 (Test for admin accessor but getting sql exceptions with check serial)
=======


            var noone = Admin.DbController.GetUser("notAUsername", "notAPassword");
            Console.WriteLine(noone.Username, noone.Password, noone.SerialNumber);


>>>>>>> 7d03c21 (fixed bad procedure variable names, fixed admin controller)
        }
    }
}
