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
                .WithLastName("Doe")
                .WithFirstName("Jane")
                .WithMiddleName("X")
                .WithLicenseNumber("A12345678")
                .Build();

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

        }
    }
}
