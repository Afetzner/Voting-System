using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingSystem.Controller;

namespace IntegrationTests.Interactive
{
    public class Menu
    {
        public static bool ConnectionTestMenu()
        {
            Console.Write("Connection to DB: ");
            bool success = false;
            int tries = 0;
            while (tries < 3)
            {
                success = DbConnecter.TestConnection();
                tries++;
                Console.WriteLine(success ? "success" : "failed");
                if (success || !TryAgainMenu())
                    break;
            }

            Console.Write(tries == 3 ? "Forced exit\n" : "");
            return success;
        }

        private static bool TryAgainMenu()
        {
            Console.Write("Try again? (Y/N): ");
            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    'y' => true,
                    'Y' => true,
                    '1' => true,
                    _ => false
                };
            }
        }

        public static bool Exit()
        {
            return false;
        }

        public static Func<bool> MetaExit()
        {
            return Exit;
        }

        public static Func<Func<bool>> SelectTestGroupMenu()
        {
            Console.WriteLine("Select a group to test");
            Console.WriteLine(
                " (0) Initialize Db\n" +
                " (1) Auto all tests\n" + 
                " (2) Admin\n" +
                " (3) Voter\n" +
                " (4) Ballot-Issue\n" +
                " (5) Ballot-Issue Option\n" +
                " (6) Results\n" +
                " (7) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => DbInitializers.DbInitMenu,
                    '1' => VoterTests.VoterTestMenu,
                    '2' => AdminTests.AdminTestMenu,
                    '3' => null,
                    '4' => null,
                    '5' => null,
                    '6' => null,
                    '7' => MetaExit,
                    _ => MetaExit
                };
            }
        }
    }
}
