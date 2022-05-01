using System;
using IntegrationTests.Interactive;
using System.Collections.Generic;
using VotingSystem.Model;

namespace IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Integration Testing Console:");
            if (!Menu.ConnectionTestMenu())
                return;

            bool success = true;
            while (success)
            {
                var groupSelection = Menu.SelectTestGroupMenu();
                var actionSelection = groupSelection();
                success = actionSelection();
            }
            Console.WriteLine("\nExiting...");

            //bool tester = BallotTests.TestAddBallot();
            //Console.WriteLine("This is tester: ", tester);

            //bool tester2 = BallotTests.TestGetBallot();
            //Console.WriteLine("This is tester2: ", tester2);

            //bool tester3 = BallotTests.TestDeleteBallot();
            //Console.WriteLine("This is tester3: ", tester3);

            //bool tester4 = BallotTests.TestAddDuplicateBallotVoterIssueChoice();
            //Console.WriteLine("This is tester4: ", tester4);
        }
    }
}
