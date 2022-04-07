using System;

namespace CSCE361_voting_system
{
    class Program
    {
        static void Main(string[] args)
        {
            Voter alex = new VoterBuilder().withFirstName("Alex")
                .withLastName("Fetzner")
                .withMiddleName("N")
                .withLicenseNumber("123abc")
                .Build();
            Console.WriteLine("Hello World!");
        }
    }
}
