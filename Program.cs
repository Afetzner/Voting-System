using System;

namespace CSCE361_voting_system.Model
{
    class Program
    {
        static void Main(string[] args)
        {
            Voter alex = new VoterBuilder().WithFirstName("Alex")
                .WithLastName("Fetzner")
                .WithMiddleName("N")
                .WithLicenseNumber("123abc")
                .Build();
            Console.WriteLine("Hello World!");
        }
    }
}
