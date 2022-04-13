using System;
using MySql.Data.MySqlClient;
using VotingSystem.Controller;
using VotingSystem.Model;


namespace VotingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
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
            
        }
    }
}
