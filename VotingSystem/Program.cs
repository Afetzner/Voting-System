using System;
<<<<<<< HEAD
using MySql.Data.MySqlClient;
using VotingSystem.Controller;
using VotingSystem.Model;

=======
using VotingSystem.Controller;
>>>>>>> 2149257 (added validateDate tests)

namespace VotingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine(ValidationUtils.IsValidDate("2999-12-31"));
            Console.WriteLine(ValidationUtils.IsValidDate("2000-01-01"));
>>>>>>> 2149257 (added validateDate tests)
        }
    }
}
