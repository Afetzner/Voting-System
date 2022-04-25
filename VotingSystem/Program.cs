using System;
<<<<<<< HEAD
<<<<<<< HEAD
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
                .WithPassword("D0eN0ADear!")
                .WithLastName("Doe")
                .WithFirstName("Jane")
                .WithSerialNumber("A12345678")
                .Build();

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
        }
    }
}
